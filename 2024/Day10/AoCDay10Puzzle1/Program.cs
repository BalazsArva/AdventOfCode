namespace AoCDay10Puzzle1;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var map = await ReadInputAsync();
        var result = Solve(map);

        Console.WriteLine(result);
    }

    private static async Task<TopographicMap> ReadInputAsync()
    {
        var lines = await File.ReadAllLinesAsync("Inputs\\input.txt");
        var heights = new int[lines.Length, lines[0].Length];

        for (var row = 0; row < lines.Length; ++row)
        {
            for (var col = 0; col < lines[row].Length; ++col)
            {
                heights[row, col] = lines[row][col] - '0';
            }
        }

        return new(heights);
    }

    private static int Solve(TopographicMap map)
    {
        var startingPositions = GetTrailHeads(map);
        var total = 0;

        foreach (var position in startingPositions)
        {
            total += GetReachablePeaks(position, map).Distinct().Count();
        }

        return total;
    }

    private static IReadOnlyList<(int Row, int Col)> GetReachablePeaks((int Row, int Col) startingPosition, TopographicMap map)
    {
        var reachablePeaks = new List<(int Row, int Col)>();
        var (row, col) = startingPosition;
        if (map.Heights[row, col] == 9)
        {
            reachablePeaks.Add((row, col));
            return reachablePeaks;
        }

        foreach (var adjacentPosition in map.GetAdjacentHeights(row, col))
        {
            if (map.Heights[row, col] + 1 == adjacentPosition.Height)
            {
                reachablePeaks.AddRange(GetReachablePeaks((adjacentPosition.Row, adjacentPosition.Col), map));
            }
        }

        return reachablePeaks;
    }

    private static IReadOnlyList<(int Row, int Col)> GetTrailHeads(TopographicMap map)
    {
        var startingPositions = new List<(int Row, int Col)>();

        for (var row = 0; row < map.Heights.GetLength(0); ++row)
        {
            for (var col = 0; col < map.Heights.GetLength(1); ++col)
            {
                if (map.Heights[row, col] == 0)
                {
                    startingPositions.Add((row, col));
                }
            }
        }

        return startingPositions;
    }
}

record TopographicMap(int[,] Heights)
{
    public IReadOnlyList<(int Height, int Row, int Col)> GetAdjacentHeights(int row, int col)
    {
        var result = new List<(int Height, int Row, int Col)>();

        if (row > 0)
        {
            result.Add((Heights[row - 1, col], row - 1, col));
        }

        if (row < Heights.GetLength(0) - 1)
        {
            result.Add((Heights[row + 1, col], row + 1, col));
        }

        if (col > 0)
        {
            result.Add((Heights[row, col - 1], row, col - 1));
        }

        if (col < Heights.GetLength(1) - 1)
        {
            result.Add((Heights[row, col + 1], row, col + 1));
        }

        return result;
    }
}