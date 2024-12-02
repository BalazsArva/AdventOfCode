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
                    var dampenedLevels = report.Levels.Where((_, idx) => i != idx).ToList();

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