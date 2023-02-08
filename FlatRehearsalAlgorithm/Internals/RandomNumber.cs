namespace FlatRehearsalAlgorithm;

internal static class RandomNumber
{
    private static readonly Random _r = new();

    public static int NextMinMaxInclusive(int min, int max)
    {
        return _r.Next(min, max + 1);
    }

    public static int Next(int inclusiveMin, int exclusiveMax)
    {
        return _r.Next(inclusiveMin, exclusiveMax);
    }

    public static bool TossCoin()
    {
        return NextMinMaxInclusive(0, 1) == 1;
    }

    public static double Next()
    {
        return _r.Next();
    }

    public static T GetRandomFair<T>(this IList<T> source)
    {
        int index = _r.Next(0, source.Count);
        return source[index];
    }
}
