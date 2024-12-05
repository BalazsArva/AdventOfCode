namespace AoCDay5Puzzle2;

// 4230
internal class Program
{
    private static async Task Main(string[] args)
    {
        var input = await ReadInputAsync();
        var sum = 0;

        foreach (var update in input.Updates)
        {
            var fixedUpdate = FixUpdatePages(update, input.PageOrderingRules);

            if (fixedUpdate.WasBroken)
            {
                sum += GetMiddleUpdatedPage(fixedUpdate.Update);
            }
        }

        Console.WriteLine(sum);
    }

    private static (Update Update, bool WasBroken) FixUpdatePages(Update update, IReadOnlyList<PageOrderingRule> rules)
    {
        var updatedPages = update.UpdatedPages.ToList();
        var wasBroken = false;

        while (true)
        {
            var pair = GetFirstOffendingIndexPairsOfInvalidUpdatePages(updatedPages, rules);
            if (pair == (-1, -1))
            {
                break;
            }

            wasBroken = true;

            var a = updatedPages[pair.Left];
            var b = updatedPages[pair.Right];
            updatedPages[pair.Left] = b;
            updatedPages[pair.Right] = a;
        }

        var result = update with
        {
            UpdatedPages = updatedPages,
        };

        return (result, wasBroken);
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

    private static (int Left, int Right) GetFirstOffendingIndexPairsOfInvalidUpdatePages(
        IReadOnlyList<int> updatedPages,
        IReadOnlyList<PageOrderingRule> rules)
    {
        for (var i = 0; i < updatedPages.Count; i++)
        {
            var currentPage = updatedPages[i];
            for (var j = i + 1; j < updatedPages.Count; j++)
            {
                var succeedingPage = updatedPages[j];
                if (rules.Any(r => r.PreceedingPageNumber == succeedingPage && r.SucceedingPageNumber == currentPage))
                {
                    return (i, j);
                }
            }
        }

        return (-1, -1);
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