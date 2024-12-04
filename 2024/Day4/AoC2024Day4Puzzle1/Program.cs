namespace AoC2024Day4Puzzle1;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var charMatrix = TransformInput(await File.ReadAllLinesAsync("Inputs\\input.txt"));
        var count = 0;
        var cols = charMatrix.GetLength(0);
        var rows = charMatrix.GetLength(1);

        for (var x = 0; x < cols; ++x)
        {
            for (var y = 0; y < rows; ++y)
            {
                if (charMatrix[x, y] == 'X')
                {
                    count += EnumerateNCharacterStringsInAllDirectionsFromPosition(charMatrix, x, y, 4).Count(word => word == "XMAS" || word == "SAMX");
                }
            }
        }

        Console.WriteLine(count);
    }

    private static char[,] TransformInput(string[] inputLines)
    {
        var lines = inputLines.Length;
        var columns = inputLines[0].Length;
        var result = new char[columns, lines];

        for (int y = 0; y < inputLines.Length; y++)
        {
            for (var x = 0; x < columns; ++x)
            {
                result[x, y] = inputLines[y][x];
            }
        }

        return result;
    }

    private static IEnumerable<string> EnumerateNCharacterStringsInAllDirectionsFromPosition(char[,] input, int x, int y, int numberOfChars)
    {
        var cols = input.GetLength(0);
        var rows = input.GetLength(1);
        var range = Enumerable.Range(0, numberOfChars).ToList();

        bool isValidPosition((int X, int Y) pos) => pos.X >= 0 && pos.X < cols && pos.Y >= 0 && pos.Y < rows;

        var indicesTowardsLeftFromPos = range.Select(offset => (X: x - offset, Y: y));
        var indicesTowardsRightFromPos = range.Select(offset => (X: x + offset, Y: y));
        var indicesTowardsTopFromPos = range.Select(offset => (X: x, Y: y - offset));
        var indicesTowardsBottomFromPos = range.Select(offset => (X: x, Y: y + offset));

        var indicesTowardsTopLeftFromPos = range.Select(offset => (X: x - offset, Y: y - offset));
        var indicesTowardsTopRightFromPos = range.Select(offset => (X: x - offset, Y: y + offset));
        var indicesTowardsBottomLeftFromPos = range.Select(offset => (X: x + offset, Y: y - offset));
        var indicesTowardsBottomRightFromPos = range.Select(offset => (X: x + offset, Y: y + offset));

        var allIndices = new[]
        {
            indicesTowardsLeftFromPos,
            indicesTowardsRightFromPos,
            indicesTowardsTopFromPos,
            indicesTowardsBottomFromPos,
            indicesTowardsTopLeftFromPos,
            indicesTowardsTopRightFromPos,
            indicesTowardsBottomLeftFromPos,
            indicesTowardsBottomRightFromPos,
        };

        return allIndices
            .Select(idxs => new string(idxs.TakeWhile(isValidPosition).Select(idx => input[idx.X, idx.Y]).ToArray()))
            .Where(str => str.Length == numberOfChars);
    }
}