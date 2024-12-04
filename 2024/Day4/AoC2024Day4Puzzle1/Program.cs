namespace AoC2024Day4Puzzle1;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var input = await File.ReadAllLinesAsync("Inputs\\input.txt");
        var charMatrix = TransformInput(input);

        var count = 0;

        var cols = charMatrix.GetLength(0);
        var rows = charMatrix.GetLength(1);

        for (var x = 0; x < cols; ++x)
        {
            for (var y = 0; y < rows; ++y)
            {
                if (charMatrix[x, y] == 'X')
                {
                    var wordsInAllDirections = EnumerateNCharactersInAllDirectionFromPosition(charMatrix, x, y, 4);

                    count += wordsInAllDirections.Count(word => word == "XMAS" || word == "SAMX");
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

    private static IEnumerable<string> EnumerateNCharactersInAllDirectionFromPosition(char[,] input, int x, int y, int numberOfChars)
    {
        var cols = input.GetLength(0);
        var rows = input.GetLength(1);

        var range = Enumerable.Range(0, numberOfChars).ToList();

        bool isValidPosition((int X, int Y) pos) => pos.X >= 0 && pos.X < cols && pos.Y >= 0 && pos.Y < rows;

        var indicesTowardsLeftFromPos = range.Select(offsetX => (X: x - offsetX, Y: y)).TakeWhile(isValidPosition);
        var indicesTowardsRightFromPos = range.Select(offsetX => (X: x + offsetX, Y: y)).TakeWhile(isValidPosition);
        var indicesTowardsTopFromPos = range.Select(offsetY => (X: x, Y: y - offsetY)).TakeWhile(isValidPosition);
        var indicesTowardsBottomFromPos = range.Select(offsetY => (X: x, Y: y + offsetY)).TakeWhile(isValidPosition);

        var indicesTowardsTopLeftFromPos = range.Select(offset => (X: x - offset, Y: y - offset)).TakeWhile(isValidPosition);
        var indicesTowardsTopRightFromPos = range.Select(offset => (X: x - offset, Y: y + offset)).TakeWhile(isValidPosition);
        var indicesTowardsBottomLeftFromPos = range.Select(offset => (X: x + offset, Y: y - offset)).TakeWhile(isValidPosition);
        var indicesTowardsBottomRightFromPos = range.Select(offset => (X: x + offset, Y: y + offset)).TakeWhile(isValidPosition);

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

        return allIndices.Select(idxs => new string(idxs.Select(idx => input[idx.X, idx.Y]).ToArray()));
    }
}