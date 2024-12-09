namespace AoCDay8Puzzle1;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var map = await ReadMapAsync();

        var s = Solve(map);
        var total = 0;

        for (var row = 0; row < s.GetLength(0); ++row)
        {
            for (var col = 0; col < s.GetLength(1); ++col)
            {
                var hasNode = s[row, col];
                if (hasNode)
                {
                    Console.Write('#');
                    ++total;
                }
                else
                {
                    Console.Write('.');
                }
            }
            Console.WriteLine();
        }

        Console.WriteLine(total);
    }

    private static async Task<InputMap> ReadMapAsync()
    {
        var lines = await File.ReadAllLinesAsync("Inputs\\input.txt");
        var fields = new InputField[lines.Length, lines[0].Length];

        for (int row = 0; row < lines.Length; row++)
        {
            var line = lines[row];
            if (!string.IsNullOrWhiteSpace(line))
            {
                for (int col = 0; col < line.Length; col++)
                {
                    fields[row, col] = InputField.Parse(line[col]);
                }
            }
        }

        return new InputMap(fields);
    }

    private static bool[,] Solve(InputMap map)
    {
        var frequencyPositionLookup = CreateFrequencyPositionLookup(map);
        var hasNode = new bool[map.Fields.GetLength(0), map.Fields.GetLength(1)];

        bool isValidPosition(int row, int col)
        {
            return row >= 0 && col >= 0 && row < map.Fields.GetLength(0) && col < map.Fields.GetLength(1);
        }

        foreach (var antennaPositions in frequencyPositionLookup.Values)
        {
            for (var i = 0; i < antennaPositions.Count - 1; ++i)
            {
                for (var j = i + 1; j < antennaPositions.Count; ++j)
                {
                    var iRow = antennaPositions[i].row;
                    var iCol = antennaPositions[i].col;
                    var jRow = antennaPositions[j].row;
                    var jCol = antennaPositions[j].col;

                    var vDistance = iRow - jRow;
                    var hDistance = iCol - jCol;

                    var node1Row = iRow + vDistance;
                    var node1Col = iCol + hDistance;
                    var node2Row = iRow - (2 * vDistance);
                    var node2Col = iCol - (2 * hDistance);

                    if (isValidPosition(node1Row, node1Col))
                    {
                        hasNode[node1Row, node1Col] = true;
                    }
                    if (isValidPosition(node2Row, node2Col))
                    {
                        hasNode[node2Row, node2Col] = true;
                    }
                }
            }
        }

        return hasNode;
    }

    private static Dictionary<char, List<(int row, int col)>> CreateFrequencyPositionLookup(InputMap map)
    {
        var frequencyPositionLookup = new Dictionary<char, List<(int row, int col)>>();

        for (var row = 0; row < map.Fields.GetLength(0); ++row)
        {
            for (var col = 0; col < map.Fields.GetLength(1); ++col)
            {
                if (map.Fields[row, col] is AntennaField a)
                {
                    frequencyPositionLookup.TryAdd(a.FrequencySign, []);
                    frequencyPositionLookup[a.FrequencySign].Add((row, col));
                }
            }
        }

        return frequencyPositionLookup;
    }
}