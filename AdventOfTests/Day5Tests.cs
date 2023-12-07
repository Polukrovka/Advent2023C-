using AdventOfCode;

namespace AdventOfTests;

public class Day5Tests
{
    [Test]
    public void TestParseInput()
    {
        var input = """
                    seeds: 79 14 55 13

                    seed-to-soil map:
                    50 98 2
                    52 50 48

                    soil-to-fertilizer map:
                    0 15 37
                    37 52 2
                    39 0 15
                    """;

        var (seeds, almanacMap) = Day5.ParseInput(input);

        Assert.That(seeds, Is.EquivalentTo(new[] { 79, 14, 55, 13 }));
        Assert.That(almanacMap[0],
            Is.EquivalentTo(new[] { new AlmanacRange(98, 50, 2), new AlmanacRange(50, 52, 48) }));
        Assert.That(almanacMap[1],
            Is.EquivalentTo(new[] { new AlmanacRange(15, 0, 37), new AlmanacRange(52, 37, 2), new AlmanacRange(0,39, 15) }));
    }

    [Test]
    public void TestConvertSeedToSeedRange()
    {
        var seeds = new long[] { 79, 14, 55, 13 };

        var result = Day5.ConvertSeedsToSeedRanges(seeds).ToArray();
        
        Assert.That(result[0], Is.EqualTo(new SeedRange(79, 93)));
        Assert.That(result[1], Is.EqualTo(new SeedRange(55, 68)));
    }

    [TestCase(79, 81)]
    [TestCase(14, 53)]
    [TestCase(55, 57)]
    [TestCase(13, 52)]
    public void TestMultipleSuccessive(long seed, long expectedLocation)
    {
        var almanacRanges = new List<List<AlmanacRange>>();
        almanacRanges.Add(new[] { new AlmanacRange(98, 50, 2), new AlmanacRange(50, 52, 48) }.ToList());
        almanacRanges.Add(new[] { new AlmanacRange(15, 0, 37), new AlmanacRange(52, 37, 2), new AlmanacRange(0,39, 15) }.ToList());

        var result = Day5.MapToLocation(seed, almanacRanges);
        
        Assert.That(result, Is.EqualTo(expectedLocation));
    }

    [Test]
    public void TestSingleMapping()
    {
        var almanacRanges = new List<AlmanacRange>();
        almanacRanges.AddRange(new[] { new AlmanacRange(10, 50, 2), new AlmanacRange(50, 10, 2) }.ToList());
        Assert.Multiple(() =>
        {
            Assert.That(Day5.MapToNext(9, almanacRanges), Is.EqualTo(9));
            Assert.That(Day5.MapToNext(10, almanacRanges), Is.EqualTo(50));
            Assert.That(Day5.MapToNext(11, almanacRanges), Is.EqualTo(51));
            Assert.That(Day5.MapToNext(12, almanacRanges), Is.EqualTo(12));
            Assert.That(Day5.MapToNext(50, almanacRanges), Is.EqualTo(10));
            Assert.That(Day5.MapToNext(51, almanacRanges), Is.EqualTo(11));
        });
    }

    [Test]
    public void TestMapSeedRangeToNext1()
    {
        var seedRange = new SeedRange(79, 93);

        var result = Day5.MapSeedRangeToNext(seedRange, new List<AlmanacRange>());
        
        Assert.That(result[0], Is.EqualTo(seedRange));
    }
    
    [Test]
    public void TestMapSeedRangeToNext2()
    {
        var seedRange = new SeedRange(79, 93);
        var almanacRanges = new List<AlmanacRange>();
        almanacRanges.AddRange(new[] { new AlmanacRange(10, 50, 2), new AlmanacRange(50, 10, 2) }.ToList());
        
        var result = Day5.MapSeedRangeToNext(seedRange, almanacRanges);
        
        Assert.That(result[0], Is.EqualTo(seedRange));
    }
    
    [Test]
    public void TestMapSeedRangeToNext3()
    {
        var seedRange = new SeedRange(79, 93);
        var almanacRanges = new List<AlmanacRange>();
        almanacRanges.AddRange(new[] { new AlmanacRange(79, 50, 14), new AlmanacRange(50, 79, 14) }.ToList());
        
        var result = Day5.MapSeedRangeToNext(seedRange, almanacRanges);
        
        Assert.That(result[0], Is.EqualTo(new SeedRange(50, 64)));
    }
    
    [Test]
    public void TestMapSeedRangeToNext4()
    {
        var seedRange = new SeedRange(10, 30);
        var almanacRanges = new List<AlmanacRange>();
        almanacRanges.AddRange(new[] { new AlmanacRange(10, 50, 10), new AlmanacRange(50, 10, 10) }.ToList());
        
        var result = Day5.MapSeedRangeToNext(seedRange, almanacRanges);
        
        Assert.That(result[1], Is.EqualTo(new SeedRange(50, 60)));
        Assert.That(result[0], Is.EqualTo(new SeedRange(20, 30)));
    }
    
    [Test]
    public void TestMapSeedRangeToNext5()
    {
        var seedRange = new SeedRange(10, 30);
        var almanacRanges = new List<AlmanacRange>();
        almanacRanges.AddRange(new[] { new AlmanacRange(20, 50, 10), new AlmanacRange(50, 20, 10) }.ToList());
        
        var result = Day5.MapSeedRangeToNext(seedRange, almanacRanges);
        
        Assert.That(result[1], Is.EqualTo(new SeedRange(50, 60)));
        Assert.That(result[0], Is.EqualTo(new SeedRange(10, 20)));
    }
    
    [Test]
    public void TestMapSeedRangeToNext6()
    {
        var seedRange = new SeedRange(10, 30);
        var almanacRanges = new List<AlmanacRange>();
        almanacRanges.AddRange(new[] { new AlmanacRange(15, 50, 10), new AlmanacRange(50, 15, 10) }.ToList());
        
        var result = Day5.MapSeedRangeToNext(seedRange, almanacRanges);
        Assert.Multiple(() =>
        {
            Assert.That(result[0], Is.EqualTo(new SeedRange(10, 15)));
            Assert.That(result[2], Is.EqualTo(new SeedRange(50, 60)));
            Assert.That(result[1], Is.EqualTo(new SeedRange(25, 30)));
        });
    }
}