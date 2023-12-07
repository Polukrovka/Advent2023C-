using AdventOfCode;

namespace AdventOfTests;

public class Day3Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestParseMap()
    {
        var input = new[]
        {
            "467.......",
            "...*......",
            ".......33.",
            "......#..."
        };

        var result = Day3.ParseMap(input);

        Assert.That(result.symbols,
            Is.EquivalentTo(new[] { new Symbol('*', new Point(3, 1)), new Symbol('#', new Point(6, 3)) }));
        Assert.That(result.numbers[0].Value, Is.EqualTo(467));
        Assert.That(result.numbers[1].Value, Is.EqualTo(33));
        Assert.That(result.numbers[0].points,
            Is.EquivalentTo(new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }));
        Assert.That(result.numbers[1].points, Is.EquivalentTo(new[] { new Point(7, 2), new Point(8, 2) }));
    }

    [Test]
    public void TestAreTouching([Values(0,1,2,3,4)] int x, [Values(0,1,2)] int y)
    {
        var number = new Number(467, new[] { new Point(1, 1), new Point(2, 1), new Point(3, 1) });
        var symbol = new Symbol('*', new Point(x, y));
        
        Assert.That(Day3.AreTouching(number, symbol), Is.True);
    }
}