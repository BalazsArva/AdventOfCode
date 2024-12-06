namespace AoCDay6Puzzle2;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var mapDescriptor = await ReadMapDescriptor();

        NormalRun(mapDescriptor);
    }

    private static void NormalRun(MapDescriptor mapDescriptor)
    {
        var referenceMap = new Map(
            mapDescriptor.Rows,
            mapDescriptor.Cols,
            mapDescriptor.ObstaclePositions,
            mapDescriptor.GuardPosition);

        while (referenceMap.MoveGuard() == Map.MovementResult.MovedSuccessfully) ;

        var referencePathTaken = referenceMap.GuardPath.ToHashSet();
        var couldCauseLoopCount = 0;
        var processed = 0L;
        var total = referenceMap.GuardPath.Count;

        Parallel.ForEach(
            referencePathTaken,
            element =>
            {
                var row = element.Row;
                var col = element.Col;

                if (mapDescriptor.GuardPosition == (row, col) ||
                    mapDescriptor.ObstaclePositions.Contains((row, col)))
                {
                    Interlocked.Increment(ref processed);
                    var currProcessed = Interlocked.Read(ref processed);

                    Console.WriteLine($"Row {row} Col {col} occupied, processed {currProcessed} / {total}");

                    return;
                }

                var map = new Map(
                    mapDescriptor.Rows,
                    mapDescriptor.Cols,
                    mapDescriptor.ObstaclePositions.Append((row, col)).ToList(),
                    mapDescriptor.GuardPosition);

                while (true)
                {
                    var result = map.MoveGuard();
                    if (result == Map.MovementResult.MovedOffMap)
                    {
                        Interlocked.Increment(ref processed);
                        var currProcessed = Interlocked.Read(ref processed);

                        Console.WriteLine($"Row {row} Col {col} moved off map, processed {currProcessed} / {total}");

                        return;
                    }
                    else if (result == Map.MovementResult.EnteredLoop)
                    {
                        Interlocked.Increment(ref couldCauseLoopCount);
                        Interlocked.Increment(ref processed);

                        var currProcessed = Interlocked.Read(ref processed);

                        Console.WriteLine($"Row {row} Col {col} entered loop, processed {currProcessed} / {total}");

                        return;
                    }
                }
            });

        Console.WriteLine();
        Console.WriteLine(couldCauseLoopCount);
    }

    private static async Task<MapDescriptor> ReadMapDescriptor()
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

        return new(currentLine, cols, obstaclePositions, (guardRow, guardCol));
    }
}

record MapDescriptor(int Rows, int Cols, IReadOnlyList<(int Row, int Col)> ObstaclePositions, (int Row, int Col) GuardPosition);