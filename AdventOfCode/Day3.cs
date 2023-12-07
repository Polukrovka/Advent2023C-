namespace AdventOfCode;

public class Day3
{
    public static void Run()
    {
        var input = File.ReadLines("in3.txt");
        var (symbols, numbers) = ParseMap(input.ToArray());

        var leftovers = numbers
            .Where(number => symbols
                .Any(symbol => AreTouching(number, symbol)))
            .Sum(n => n.Value);
        
        Console.WriteLine(leftovers);

        var gears = symbols.Where(symbol =>
            symbol.Value == '*' && numbers.Where(number => AreTouching(number, symbol)).Count() == 2);
        var ratio = gears
            .Select(gear => numbers.Where(number => AreTouching(number, gear)))
            .Select(numbers => numbers.First().Value * numbers.Last().Value)
            .Sum();
        
        Console.WriteLine(ratio);
    }

    public static (Symbol[] symbols, Number[] numbers) ParseMap(string[] input)
    {
        var symbols = new List<Symbol>();
        var numbers = new List<Number>();
        for (int line = 0; line < input.Length; line++)
        {
            var trimmed = input[line].Trim();
            for (int i = 0; i < trimmed.Length; i++)
            {
                var num = "";
                var points = new List<Point>();
                if (trimmed[i] == '.')
                    continue;
                while (Char.IsDigit(trimmed[i]))
                {
                    num += trimmed[i];
                    points.Add(new Point(i, line));
                    i++;
                    if (i == trimmed.Length || !Char.IsDigit(trimmed[i]) )
                    {
                        numbers.Add(new Number(Int32.Parse(num), points.ToArray()));
                        i--;
                        break;
                    }
                }

                if (string.IsNullOrEmpty(num))
                    symbols.Add(new(trimmed[i], new(i, line)));
            }
        }

        return (symbols.ToArray(), numbers.ToArray());
    }

    public static bool AreTouching(Number number, Symbol symbol)
    {
        return number.points.Any(point =>
            Math.Abs(point.X - symbol.point.X) <= 1 && Math.Abs(point.Y - symbol.point.Y) <= 1);
    }
}

public record Point(int X, int Y);

public record Symbol(char Value, Point point);

public record Number(int Value, Point[] points);