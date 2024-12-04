namespace AoC2024Day4Puzzle2;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var charMatrix = TransformInput(await File.ReadAllLinesAsync("Inputs\\input.txt"));
        var count = 0;
        var cols = charMatrix.GetLength(0);
        var rows = charMatrix.GetLength(1);

        for (var x = 1; x < cols - 1; ++x)
        {
            for (var y = 1; y < rows - 1; ++y)
            {
                if (charMatrix[x, y] == 'A')
                {
                    var resultD = Get3LongDiagonalStringsCrossingAtPosition(charMatrix, x, y);
                    if ((resultD.Diagonal1 is "MAS" or "SAM") && (resultD.Diagonal2 is "MAS" or "SAM"))
                    {
                        count++;
                    }
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

    private static (string Diagonal1, string Diagonal2) Get3LongDiagonalStringsCrossingAtPosition(
        char[,] input, int x, int y)
    {
        var diag1Idxs = new[]
        {
            (X: x - 1, Y: y - 1),
            (X: x - 0, Y: y - 0),
            (X: x + 1, Y: y + 1),
        };
        var diag2Idxs = new[]
        {
            (X: x - 1, Y: y + 1),
            (X: x - 0, Y: y - 0),
            (X: x + 1, Y: y - 1),
        };

        var diag1 = new string(diag1Idxs.Select(pos => input[pos.X, pos.Y]).ToArray());
        var diag2 = new string(diag2Idxs.Select(pos => input[pos.X, pos.Y]).ToArray());

        return (diag1, diag2);
    }
}