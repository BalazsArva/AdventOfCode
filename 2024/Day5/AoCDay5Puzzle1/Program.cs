namespace AoCDay5Puzzle1;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var input = await ReadInputAsync();
        var sum = 0;

        foreach (var update in input.Updates)
        {
            if (IsValidUpdate(update, input.PageOrderingRules))
            {
                sum += GetMiddleUpdatedPage(update);
            }
        }

        Console.WriteLine(sum);
    }

    private static async Task<InputFile> ReadInputAsync()
    {
        var rules = new List<PageOrderingRule>();
        var updates = new List<Update>();
        var readingUpdatesSegment = false;
        using var reader = new StreamReader("Inputs\\input.txt");

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();

            if (string.IsNullOrWhiteSpace(line))
            {
                readingUpdatesSegment = true;
                continue;
            }

            if (readingUpdatesSegment)
            {
                updates.Add(Update.Parse(line));
            }
            else
            {
                rules.Add(PageOrderingRule.Parse(line));
            }
        }

        return new(rules, updates);
    }

    private static bool IsValidUpdate(Update update, IReadOnlyList<PageOrderingRule> rules)
    {
        for (var i = 0; i < update.UpdatedPages.Count; i++)
        {
            var currentPage = update.UpdatedPages[i];
            for (var j = i + 1; j < update.UpdatedPages.Count; j++)
            {
                var succeedingPage = update.UpdatedPages[j];
                if (rules.Any(r => r.PreceedingPageNumber == succeedingPage && r.SucceedingPageNumber == currentPage))
                {
                    return false;
                }
            }
        }

        return true;
    }

    private static int GetMiddleUpdatedPage(Update update)
    {
        var total = update.UpdatedPages.Count;
        var idx = total == 1 ? 1 : total / 2;

        return update.UpdatedPages[idx];
    }
}

record InputFile(IReadOnlyList<PageOrderingRule> PageOrderingRules, IReadOnlyList<Update> Updates);

record PageOrderingRule(int PreceedingPageNumber, int SucceedingPageNumber)
{
    public static PageOrderingRule Parse(string input)
    {
        var pgNumbers = input.Split('|', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        return new(
            int.Parse(pgNumbers[0]),
            int.Parse(pgNumbers[1]));
    }
}

record Update(IReadOnlyList<int> UpdatedPages)
{
    public static Update Parse(string input)
    {
        var pgNumbers = input.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        return new(pgNumbers.Select(int.Parse).ToList());
    }
}