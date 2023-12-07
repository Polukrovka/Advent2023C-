using AdventOfCode;

namespace AdventOfTests;

public class Day4Tests
{
    [Test]
    public void TestScratcherParse()
    {
        var day4 = new Day4();
        var scratcher = "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53";

        var (winningNumbers, scratchedNumbers) = day4.ParseScratcher(scratcher);

        Assert.That(winningNumbers, Is.EquivalentTo(new[] { 41, 48, 83, 86, 17 }));
        Assert.That(scratchedNumbers, Is.EquivalentTo(new[] { 83, 86, 6, 31, 17, 9, 48, 53 }));
    }

    [TestCase(0, 0)]
    [TestCase(1, 1)]
    [TestCase(2, 2)]
    [TestCase(3, 4)]
    [TestCase(4, 8)]
    public void TestCalculatePoints(int numberInCommon, int expectedPoints)
    {
        var day4 = new Day4();
        var commonNumbers = Enumerable.Range(0, numberInCommon).ToArray();

        var points = day4.CalculatePoints(commonNumbers);

        Assert.That(points, Is.EqualTo(expectedPoints));
    }

    [Test]
    public void TestCalculateCopiesZero()
    {
        var day4 = new Day4();
        var copiesWon = new [] { 0 };
        
        var copies = day4.CalculateCopies(copiesWon);
        
        Assert.That(copies, Is.EqualTo(1));
    }
    
    [Test]
    public void TestCalculateCopiesOne()
    {
        var day4 = new Day4();
        var copiesWon = new [] { 99 };
        
        var copies = day4.CalculateCopies(copiesWon);
        
        Assert.That(copies, Is.EqualTo(1));
    }
    
    [Test]
    public void TestCalculateCopiesZeroZero()
    {
        var day4 = new Day4();
        var copiesWon = new [] { 0, 0 };
        
        var copies = day4.CalculateCopies(copiesWon);
        
        Assert.That(copies, Is.EqualTo(2));
    }

    [Test]
    public void TestCalculateCopiesOneZero()
    {
        var day4 = new Day4();
        var copiesWon = new [] { 1, 0 };
        
        var copies = day4.CalculateCopies(copiesWon);
        
        Assert.That(copies, Is.EqualTo(3));
    }
    
    [Test]
    public void TestCalculateCopiesOneOne()
    {
        var day4 = new Day4();
        var copiesWon = new [] { 1, 1 };
        
        var copies = day4.CalculateCopies(copiesWon);
        
        Assert.That(copies, Is.EqualTo(3));
    }
    
    [Test]
    public void TestCalculateCopiesBigTest()
    {
        var day4 = new Day4();
        var copiesWon = new [] { 4, 2, 2, 1, 0, 0 };
        
        var copies = day4.CalculateCopies(copiesWon);
        
        Assert.That(copies, Is.EqualTo(30));
    }
}