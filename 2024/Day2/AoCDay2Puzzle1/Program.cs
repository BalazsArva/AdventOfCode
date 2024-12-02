namespace AoCDay2Puzzle1;

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
        }

        Console.WriteLine(safeReportCount);
    }
}