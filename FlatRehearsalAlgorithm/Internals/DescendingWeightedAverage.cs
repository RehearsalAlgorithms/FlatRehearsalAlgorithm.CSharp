namespace FlatRehearsalAlgorithm.Internals;

internal class DescendingWeightedAverage
{
    public static decimal Of(IList<decimal> numbers) => AscendingWeightedAverage.Calculate(numbers.Reverse(), numbers.Count);
}
