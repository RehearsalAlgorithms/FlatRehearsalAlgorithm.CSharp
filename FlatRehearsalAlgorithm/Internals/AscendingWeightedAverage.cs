namespace FlatRehearsalAlgorithm.Internals;

internal class AscendingWeightedAverage
{
    public static decimal Of(IList<decimal> numbers) => Calculate(numbers, numbers.Count);
    internal static decimal Calculate(IEnumerable<decimal> numbers, int count)
    {
        int weight = 1;
        decimal sumOfAllWeights = SumOfIntegers.Until(count);
        return numbers.Sum(i => weight++ / sumOfAllWeights * i);
    }
}
