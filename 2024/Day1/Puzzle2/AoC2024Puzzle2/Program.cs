using System.Text.RegularExpressions;

namespace AoC2024Puzzle2;

public class Program
{
    public static async Task Main(string[] args)
    {
        var input =  await ReadInputAsync();

        var rightListLookup = input.Right
            .GroupBy(x => x, (key, elements) => new { Number = key, Occurrences = elements.Count() })
            .ToDictionary(x => x.Number, x => x.Occurrences);

        var result = input.Left
            .Select(num => rightListLookup.TryGetValue(num, out var occurrences) ? occurrences * num : 0)
            .Sum();

        Console.WriteLine(result);
    }

    private static async Task<(IReadOnlyCollection<int> Left, IReadOnlyCollection<int> Right)> ReadInputAsync()
    {
        var left = new List<int>(1000);
        var right = new List<int>(1000);

        using var reader = new StreamReader("Inputs\\input.txt");

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (!string.IsNullOrWhiteSpace(line))
            {
                var num1 = int.Parse(Regex.Match(line, "^\\d+").ValueSpan);
                var num2 = int.Parse(Regex.Match(line, "\\d+$").ValueSpan);

                left.Add(num1);
                right.Add(num2);
            }
        }

        return (left.OrderBy(x => x).ToList(), right.OrderBy(x => x).ToList());
    }
}
