namespace AoCDay2Puzzle1;

public static class LinqExtensions
{
    public static IEnumerable<TOut> SelectCurrentAndPrevious<TIn, TOut>(this IReadOnlyList<TIn> source, Func<TIn, TIn, TOut> selector)
    {
        for (var i = 1; i < source.Count; i++)
        {
            yield return selector(source[i], source[i - 1]);
        }
    }
}