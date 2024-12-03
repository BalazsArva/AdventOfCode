using System.Text.RegularExpressions;

namespace AoC2024Day3Puzzle2;

internal class Program
{
    private static void Main(string[] args)
    {
        var sum = 0;
        var doOrDont = true;
        var input = File.ReadAllText("Inputs\\input.txt");
        var matches = Regex.Matches(
            input,
            "(?<mul>mul\\((?<lhs>\\d{1,3}),(?<rhs>\\d{1,3})\\))|(?<do>do\\(\\))|(?<donot>don't\\(\\))",
            RegexOptions.None,
            TimeSpan.FromSeconds(10));

        foreach (Match match in matches)
        {
            if (match.Groups["do"].Success)
            {
                doOrDont = true;
            }
            else if (match.Groups["donot"].Success)
            {
                doOrDont = false;
            }
            else if (match.Groups["mul"].Success && doOrDont)
            {
                var lhs = int.Parse(match.Groups["lhs"].ValueSpan);
                var rhs = int.Parse(match.Groups["rhs"].ValueSpan);

                sum += lhs * rhs;
            }
        }

        Console.WriteLine(sum);
    }
}