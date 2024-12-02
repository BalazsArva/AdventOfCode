namespace AoCDay2Puzzle2;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var reports = ReportReader.ReadReportsAsync();
        var safeReportCount = 0;

        await foreach (var report in reports)
        {
            if (report.IsSafe())
            {
                ++safeReportCount;
            }
            else
            {
                for (var i = 0; i < report.Levels.Count; ++i)
                {
                    var dampenedLevels = new List<int>();
                    for (var j = 0; j < report.Levels.Count; ++j)
                    {
                        if (i != j)
                        {
                            dampenedLevels.Add(report.Levels[j]);
                        }
                    }

                    if (new Report(dampenedLevels).IsSafe())
                    {
                        ++safeReportCount;
                        break;
                    }
                }
            }
        }

        Console.WriteLine(safeReportCount);
    }
}