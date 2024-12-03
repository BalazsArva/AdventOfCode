using System.Text.RegularExpressions;

namespace AoC2024Day3Puzzle1;

internal class Program
{
    private static void Main(string[] args)
    {
        var input = File.ReadAllText("Inputs\\input.txt");
        var matches = Regex.Matches(input, "mul\\((?<lhs>\\d{1,3}),(?<rhs>\\d{1,3})\\)", RegexOptions.None, TimeSpan.FromSeconds(10));
        var sum = 0;

        foreach (Match match in matches.Where(x => x.Success))
        {
            var lhs = int.Parse(match.Groups["lhs"].ValueSpan);
            var rhs = int.Parse(match.Groups["rhs"].ValueSpan);

            sum += lhs * rhs;
        }

        Console.WriteLine(sum);
    }
}