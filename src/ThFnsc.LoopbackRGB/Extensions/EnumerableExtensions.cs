namespace ThFnsc.LoopbackRGB.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> source) =>
        source
            .Where(x => x != null)
            .Cast<T>();
}
