namespace AoCDay2Puzzle2;

public record Report(IReadOnlyList<int> Levels)
{
    public bool IsSafe()
    {
        var diffsToPrevious = Levels.SelectCurrentAndPrevious((curr, prev) => curr - prev).ToList();
        var signs = diffsToPrevious.Select(Math.Sign);

        if (signs.Distinct().Count() > 1)
        {
            return false;
        }

        if (diffsToPrevious.Any(x => Math.Abs(x) is < 1 or > 3))
        {
            return false;
        }

        return true;
    }
}
