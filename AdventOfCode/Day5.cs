using System.Diagnostics;

namespace AdventOfCode;

public class Day5
{
    public static void Run()
    {
        var input = File.ReadAllText("in5.txt");
        var (seeds, almanacMap) = ParseInput(input);
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        Console.WriteLine(seeds.Select(seed => MapToLocation(seed, almanacMap)).Min());
        stopwatch.Stop();
        Console.WriteLine(stopwatch.ElapsedMilliseconds + "ms");
        
        stopwatch = new Stopwatch();
        stopwatch.Start();
        var srs = ConvertSeedsToSeedRanges(seeds);
        Console.WriteLine(srs.Select(sr => MapSeedRangeToLocation(sr, almanacMap)).Min());
        stopwatch.Stop();
        Console.WriteLine(stopwatch.ElapsedMilliseconds + "ms");
    }

    public static (long[] seeds, List<List<AlmanacRange>> almanacMap) ParseInput(string input)
    {
        var entries = input.Split("\r\n\r\n");
        var seeds = entries[0].Split(": ")[1].Trim().Split(" ").Select(v => Int64.Parse(v));
        var maps = new Span<string>(entries, 1, entries.Length - 1);
        var almanacMap = new List<List<AlmanacRange>>();
        for (int i = 0; i < maps.Length; i++)
        {
            var split = maps[i].Split("\r\n");
            almanacMap.Add(new List<AlmanacRange>());
            var almanacRanges = split.Skip(1).Select(values =>
            {
                var parsed = values.Split(" ").Select(v => Int64.Parse(v)).ToArray();
                return new AlmanacRange(parsed[1], parsed[0], parsed[2]);
            });
            almanacMap[i].AddRange(almanacRanges);
        }

        return (seeds.ToArray(), almanacMap);
    }

    public static long MapToLocation(long seed, List<List<AlmanacRange>> almanacRanges)
    {
        var currentValue = seed;
        foreach (var ranges in almanacRanges)
        {
            currentValue = MapToNext(currentValue, ranges);
        }

        return currentValue;
    }
    
    public static long MapSeedRangeToLocation(SeedRange seedRange, List<List<AlmanacRange>> almanacRanges)
    {
        List<SeedRange> currentValue = new List<SeedRange>();
        currentValue.AddRange( MapSeedRangeToNext(seedRange, almanacRanges[0]));

        for (int i = 1; i< almanacRanges.Count; i++)
        {
            var futureValue = new List<SeedRange>();
            foreach (var sr in currentValue)
            {
                futureValue.AddRange(MapSeedRangeToNext(sr, almanacRanges[i]));
            }
            currentValue = futureValue;
        }

        return currentValue.Select( sr=> sr.Start).Min();
    }

    public static long MapToNext(long seed, List<AlmanacRange> almanacRanges)
    {
        var range = almanacRanges.Find(ar => ar.IsInRange(seed));
        return range?.GetMappedValue(seed) ?? seed;
    }

    public static IEnumerable<SeedRange> ConvertSeedsToSeedRanges(long[] seeds)
    {
        return seeds.Chunk(2).Select(pair => new SeedRange(pair[0], pair[0] + pair[1]));
    }

    public static List<SeedRange> MapSeedRangeToNext(SeedRange seedRange, List<AlmanacRange> almanacRanges)
    {
        var list = new List<SeedRange>();
        var range = almanacRanges.Find(ar => ar.IsInRange(seedRange.Start) || ar.IsInRange(seedRange.End-1)) ??
                    almanacRanges.Find(ar => seedRange.IsInRange(ar));
        long start, end;
        if (range is not null)
        {
            start = range.GetMappedValue(seedRange.Start);
            if (seedRange.Start < range.SourceStart)
            {
                list.AddRange(MapSeedRangeToNext(new SeedRange(seedRange.Start, range.SourceStart ), almanacRanges));
                list.AddRange(MapSeedRangeToNext(new SeedRange(range.SourceStart, seedRange.End), almanacRanges));
                return list;
            }
            if (seedRange.End <= range.SourceStart + range.Range)
                end = range.GetMappedValue(seedRange.End);
            else
            {
                end = range.DestinationStart + range.Range;
                list.AddRange(MapSeedRangeToNext(new SeedRange(end-start + seedRange.Start, seedRange.End), almanacRanges));
                list.AddRange(MapSeedRangeToNext(new SeedRange(seedRange.Start, range.SourceStart + range.Range), almanacRanges));
                return list;
            }
        }
        else
        {
            start = seedRange.Start;
            end = seedRange.End;
        }
        list.Add(new SeedRange(start,end));
        
        return list;
    }
}

public record AlmanacRange(long SourceStart, long DestinationStart, long Range)
{
    public bool IsInRange(long value) => value >= SourceStart && value < SourceStart + Range;
    public long GetMappedValue(long value) => value - SourceStart + DestinationStart;
};

public record SeedRange(long Start, long End)
{
    public bool IsInRange(AlmanacRange almanacRange) => almanacRange.SourceStart >= Start && almanacRange.SourceStart + almanacRange.Range <= End;

};