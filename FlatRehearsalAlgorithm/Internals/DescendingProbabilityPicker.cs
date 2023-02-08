namespace FlatRehearsalAlgorithm.Internals;

internal static class DescendingProbabilityPicker
{
    /// <summary>
    /// <code>
    /// length = items.Count
    /// total = length * (length + 1) / 2.
    /// </code>
    /// The item at index <c>length - 1</c> has the lowest probability of being returned: <c>1/total</c>.<br/>
    /// The item at index <c>length - 2</c> has a probability <c>2/total</c> of being returned, etc.<br/>
    /// The item at index 0 has the highest probability of being returned: <c>length/total</c>.
    /// </summary>
    /// <param name="items"></param>
    /// <returns>A semi-randomly selected item, where a lower index means a higher probability of being returned.</returns>
    public static T Pick<T>(IList<T> items)
    {
        int pickedIndex = PickIndex(items.Count);
        return items[pickedIndex];
    }

    private static int PickIndex(int length)
    {
        var total = SumOfIntegers.Until(length);
        var indexInTotal = RandomNumber.Next(0, total);
        for (int boundary = 0, i = 0; i < length; i++)
        {
            boundary += (length - i);
            if (indexInTotal < boundary)
            {
                return i;
            }
        }
        throw new ArgumentOutOfRangeException($"The combination of length = {length} and indexInHierarchy = {indexInTotal} is impossible.");
    }
}
