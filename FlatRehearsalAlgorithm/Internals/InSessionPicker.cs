namespace FlatRehearsalAlgorithm.Internals;

internal static class InSessionPicker
{
    public static TLearnItem Pick<TLearnItem>(List<TLearnItem> items, int goalScore) where TLearnItem : ILearnItem
    {
        switch (RandomNumber.NextMinMaxInclusive(1, 6))
        {
            case 1:
                return GetItemLongestNotAsked(items);
            case 2:
            case 3:
                return items.OrderDeferredForGoalScore(goalScore).First();
            default:
                items.SortForGoalScore(goalScore);
                var item = DescendingProbabilityPicker.Pick(items);
                if (item.Score > 95 && RandomNumber.TossCoin())
                {
                    return GetItemLongestNotAsked(items);
                }
                return item;
        }
    }

    private static TLearnItem GetItemLongestNotAsked<TLearnItem>(List<TLearnItem> items) where TLearnItem : ILearnItem
    {
        var item = items.FirstOrDefault(x => x.LastTimeAskedUtcTicks is null);
        if (item != null)
        {
            return item;
        }
        var minTime = items.Min(x => x.LastTimeAskedUtcTicks);
        return items.First(item => item.LastTimeAskedUtcTicks == minTime);
    }
}
