namespace TestWork2.Extensions;

public static class MedianExtensions
{
    private static double Median(this IEnumerable<int> source)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        int[] data = source.OrderBy(n => n).ToArray();
        if (data.Length == 0)
            throw new InvalidOperationException();
        if (data.Length % 2 == 0)
            return (data[data.Length / 2 - 1] + data[data.Length / 2]) / 2.0;
        return data[data.Length / 2];
    }

    public static double Median<TSource>(this IEnumerable<TSource?> source, Func<TSource, int> selector)
    {
        return source!.Select(selector).Median();
    }
}