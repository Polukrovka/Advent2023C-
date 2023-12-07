namespace AdventOfCode;

public class Day4
{
    public void Run()
    {
        var input = File.ReadLines("in4.txt").ToArray();
        // var sum = input
        //     .Select(ParseScratcher)
        //     .Select(tuple => tuple.winningNumbers.Intersect(tuple.scratchedNumbers))
        //     .Select(intersection => CalculatePoints(intersection.ToArray()))
        //     .Sum();
        //
        // Console.WriteLine(sum);

        var sum = input
            .Select(ParseScratcher)
            .Select(tuple => tuple.winningNumbers.Intersect(tuple.scratchedNumbers).ToArray().Length);
        var bigPoints = CalculateCopies(sum.ToArray());


        Console.WriteLine(bigPoints);
    }

    public (int[] winningNumbers, int[] scratchedNumbers) ParseScratcher(string scratcher)
    {
        var arrs = scratcher
            .Split(": ")[1]
            .Split(" | ")
            .Select(s => s
                .Trim()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(n => Int32.Parse(n))
                .ToArray()
            ).ToArray();
        return (arrs[0], arrs[1]);
    }

    public int CalculatePoints(int[] commonNumbers)
    {
        var length = commonNumbers.Length;
        return length == 0 ? 0 : (int)Math.Pow(2, length - 1);
    }

    public long CalculateCopies(int[] copiesWon)
    {
        var currentCopies = Enumerable
            .Range(0, copiesWon.Length)
            .Select(i => 1)
            .ToArray();
        for (int i = 0; i < copiesWon.Length; i++)
        {
            for (int j = 1; j <= copiesWon[i] && i + j < copiesWon.Length; j++)
            {
                currentCopies[i + j] += currentCopies[i];
            }
        }

        return currentCopies.Sum();
    }
}