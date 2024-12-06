namespace AoCDay6Puzzle1;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var map = await ReadMap();

        NormalRun(map);
    }

    private static void DebugRun(Map map)
    {
        do
        {
            map.Draw();
            Console.ReadKey();

            Console.Clear();
        }
        while (map.MoveGuard());

        var uniquePathPositions = map.GuardPath.Distinct().Count();

        map.Draw(true);

        Console.WriteLine();
        Console.WriteLine(uniquePathPositions);
    }

    private static void NormalRun(Map map)
    {
        while (map.MoveGuard()) ;

        var uniquePathPositions = map.GuardPath.Distinct().Count();

        map.Draw(true);

        Console.WriteLine();
        Console.WriteLine(uniquePathPositions);
    }

    private static async Task<Map> ReadMap()
    {
        using var streamReader = new StreamReader("Inputs\\input.txt");

        var obstaclePositions = new List<(int Row, int Col)>();
        var guardRow = -1;
        var guardCol = -1;
        var currentLine = 0;
        var cols = 0;

        while (!streamReader.EndOfStream)
        {
            var line = await streamReader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            cols = line.Length;

            for (var i = 0; i < line.Length; ++i)
            {
                var ch = line[i];
                if (ch == '#')
                {
                    obstaclePositions.Add((currentLine, i));
                }
                else if (ch == '^')
                {
                    guardCol = i;
                    guardRow = currentLine;
                }
            }

            ++currentLine;
        }

        return new Map(currentLine, cols, obstaclePositions, (guardRow, guardCol));
    }
}