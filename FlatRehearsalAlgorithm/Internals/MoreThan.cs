namespace FlatRehearsalAlgorithm.Internals;

internal static class _MoreThan
{
    public static bool MoreThan<T>(this IEnumerable<T> source, int count) => source.Skip(count).Any();
}
