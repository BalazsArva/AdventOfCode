using System.Text;
using System.Text.RegularExpressions;

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

        return;
        foreach (var column in EnumerateColumns(charMatrix))
        {
            // don't use (XMAS)|(SAMX) here because that will only count 1 for "XMASAMX" but the rule is that they can overlap
            count += Regex.Matches(column, "(XMAS)", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1)).Count;
            count += Regex.Matches(column, "(SAMX)", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1)).Count;
        }

        foreach (var line in input)
        {
            count += Regex.Matches(line, "(XMAS)", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1)).Count;
            count += Regex.Matches(line, "(SAMX)", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1)).Count;
        }

        foreach (var diag in EnumerateTopLeftToBottomRightDiagonals(charMatrix))
        {
            count += Regex.Matches(diag, "(XMAS)", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1)).Count;
            count += Regex.Matches(diag, "(SAMX)", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1)).Count;
        }

        foreach (var diag in EnumerateTopRightToBottomLeftDiagonals(charMatrix))
        {
            count += Regex.Matches(diag, "(XMAS)", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1)).Count;
            count += Regex.Matches(diag, "(SAMX)", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1)).Count;
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

        var range = Enumerable.Range(0, numberOfChars);

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

    private static IEnumerable<string> EnumerateColumns(char[,] input)
    {
        var cols = input.GetLength(0);
        var rows = input.GetLength(1);

        for (var x = 0; x < cols; ++x)
        {
            var stringBuilder = new StringBuilder(rows);

            for (var y = 0; y < rows; ++y)
            {
                stringBuilder.Append(input[x, y]);
            }

            yield return stringBuilder.ToString();
        }
    }

    private static IEnumerable<string> EnumerateTopLeftToBottomRightDiagonals(char[,] input)
    {
        var cols = input.GetLength(0);
        var rows = input.GetLength(1);

        // This is like this (stars)
        // ************
        // *
        // *
        // *
        // *
        var startingPositionsVertically = Enumerable.Range(0, rows);
        var startingPositionsHorizontally = Enumerable.Range(1, cols - 1);

        foreach (var verticalStartingPosition in startingPositionsVertically)
        {
            var stringBuilder = new StringBuilder();
            var offset = 0;

            while (true)
            {
                var x = offset;
                var y = verticalStartingPosition + offset;

                if (x >= cols || y >= rows)
                {
                    break;
                }

                stringBuilder.Append(input[x, y]);
                ++offset;
            }

            yield return stringBuilder.ToString();
        }

        foreach (var horizontalStartingPosition in startingPositionsHorizontally)
        {
            var stringBuilder = new StringBuilder();
            var offset = 0;

            while (true)
            {
                var x = horizontalStartingPosition + offset;
                var y = offset;

                if (x >= cols || y >= rows)
                {
                    break;
                }

                stringBuilder.Append(input[x, y]);
                ++offset;
            }

            yield return stringBuilder.ToString();
        }
    }

    private static IEnumerable<string> EnumerateTopRightToBottomLeftDiagonals(char[,] input)
    {
        var cols = input.GetLength(0);
        var rows = input.GetLength(1);

        // This is like this (stars)
        //            *
        //            *
        //            *
        //            *
        // ************
        var startingPositionsVertically = Enumerable.Range(0, rows);
        var startingPositionsHorizontally = Enumerable.Range(0, cols - 1);

        foreach (var verticalStartingPosition in startingPositionsVertically)
        {
            var stringBuilder = new StringBuilder();
            var offset = 0;

            while (true)
            {
                var x = cols - 1 - offset;
                var y = verticalStartingPosition - offset;

                if (x < 0 || y < 0)
                {
                    break;
                }

                stringBuilder.Append(input[x, y]);
                ++offset;
            }

            yield return stringBuilder.ToString();
        }

        foreach (var horizontalStartingPosition in startingPositionsHorizontally)
        {
            var stringBuilder = new StringBuilder();
            var offset = 0;

            while (true)
            {
                var x = horizontalStartingPosition - offset;
                var y = rows - 1 - offset;

                if (x < 0 || y < 0)
                {
                    break;
                }

                stringBuilder.Append(input[x, y]);
                ++offset;
            }

            yield return stringBuilder.ToString();
        }
    }
}