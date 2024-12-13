namespace AoCDay11Puzzle1;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var input = await ReadInputAsync();

        var result = Solve(input);

        Console.WriteLine(result);
    }

    private static async Task<IReadOnlyList<long>> ReadInputAsync()
    {
        var input = await File.ReadAllTextAsync("Inputs\\input.txt");

        var initialNumbers = input
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(long.Parse)
            .ToList();

        return initialNumbers;
    }

    private static int Solve(IReadOnlyList<long> input)
    {
        var workBuffer = input;

        for (var i = 0; i < 25; ++i)
        {
            workBuffer = ComputeIteration(workBuffer);
        }

        return workBuffer.Count;
    }

    private static IReadOnlyList<long> ComputeIteration(IReadOnlyList<long> input)
    {
        var result = new List<long>(input.Count);
        for (var i = 0; i < input.Count; ++i)
        {
            if (input[i] == 0)
            {
                result.Add(1);
            }
            else
            {
                var valueAsString = input[i].ToString();
                if (valueAsString.Length % 2 == 0)
                {
                    var halfLength = valueAsString.Length / 2;
                    var p1 = valueAsString[..halfLength];
                    var p2 = valueAsString[halfLength..];

                    result.Add(long.Parse(p1));
                    result.Add(long.Parse(p2));
                }
                else
                {
                    result.Add(input[i] * 2024);
                }
            }
        }

        return result;
    }
}