namespace FlatRehearsalAlgorithm.Internals;

internal static class _OrderForGoalScore
{
    public static IEnumerable<TLearnItem> OrderDeferredForGoalScore<TLearnItem>(this IEnumerable<TLearnItem> list, int goalScore) where TLearnItem : ILearnItem
    {
        return list.OrderByDescending(item => item.HasBeenStudied() && item.Score < goalScore)
                    .ThenBy(item => item.Score)
                    .ThenBy(item => item.LastTimeAskedUtcTicks)
                    .ThenBy(item => RandomNumber.Next());
    }

    public static void SortForGoalScore<TLearnItem>(this List<TLearnItem> list, int goalScore) where TLearnItem : ILearnItem
    {
        var sortedList = list.OrderDeferredForGoalScore(goalScore).ToArray();
        list.Clear();
        list.AddRange(sortedList);
    }
}
