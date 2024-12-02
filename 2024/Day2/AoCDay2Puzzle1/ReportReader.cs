namespace AoCDay2Puzzle1;

public static class ReportReader
{
    public static async IAsyncEnumerable<Report> ReadReportsAsync()
    {
        using var streamReader = new StreamReader("Inputs\\input.txt");

        while (!streamReader.EndOfStream)
        {
            var line = await streamReader.ReadLineAsync();

            if (!string.IsNullOrWhiteSpace(line))
            {
                var report = new Report(
                    line
                        .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        .Select(int.Parse)
                        .ToList());

                yield return report;
            }
        }
    }
}
