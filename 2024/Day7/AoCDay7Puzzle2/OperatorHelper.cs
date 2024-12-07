namespace AoCDay7Puzzle2;

internal static class OperatorHelper
{
    private static readonly Dictionary<int, IReadOnlyList<string>> OperatorsByLengthCache = new()
    {
        [0] = [],
        // Task says || instead of | but does not matter. Current logic expects single characters.
        [1] = ["+", "*", "|"],
    };

    public static IReadOnlyList<string> CreateAllPossibleOperatorCombinations(int length)
    {
        if (OperatorsByLengthCache.TryGetValue(length, out var cached))
        {
            return cached;
        }

        var result = new List<string>();
        var temp = CreateAllPossibleOperatorCombinations(length - 1);
        foreach (var val in temp)
        {
            result.Add($"{val}+");
            result.Add($"{val}*");
            result.Add($"{val}|");
        }

        OperatorsByLengthCache.Add(length, result);

        return result;
    }

    public static IReadOnlyList<string> CreateAllPossibleOperatorCombinationsNoRecursion(int length)
    {
        if (OperatorsByLengthCache.TryGetValue(length, out var cached))
        {
            return cached;
        }

        var processingQueue = new Queue<string>();
        var results = new List<string>();

        processingQueue.Enqueue("+");
        processingQueue.Enqueue("*");
        processingQueue.Enqueue("|");

        while (processingQueue.Count > 0)
        {
            var val = processingQueue.Dequeue();
            if (val.Length == length)
            {
                results.Add(val);
            }
            else
            {
                processingQueue.Enqueue($"{val}+");
                processingQueue.Enqueue($"{val}*");
                processingQueue.Enqueue($"{val}|");
            }
        }

        OperatorsByLengthCache.Add(length, results);

        return results;
    }
}