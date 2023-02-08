namespace FlatRehearsalAlgorithm.Tests.Internals;

public class AscendingWeightedAverageTests
{
    [Theory]
    [InlineData("", 0)]

    [InlineData("0", 0)]
    [InlineData("0|0", 0)]

    [InlineData("100", 100)]
    [InlineData("100|100", 100)]

    [InlineData("100|0|0|100", 50)]
    [InlineData("0|100|0", 33)]
    [InlineData("100|0|100", 67)]
    [InlineData("100|0|100|0", 40)]

    [InlineData("0|100", 67)]
    [InlineData("0|100|100", 83)]
    [InlineData("0|100|100|100", 90)]
    [InlineData("0|100|100|100|100", 93)]
    [InlineData("0|100|100|100|100|100", 95)]
    [InlineData("0|100|100|100|100|100|100", 96)]
    [InlineData("0|100|100|100|100|100|100|100", 97)]
    [InlineData("0|100|100|100|100|100|100|100|100", 98)]
    [InlineData("0|100|100|100|100|100|100|100|100|100", 98)]
    [InlineData("0|100|100|100|100|100|100|100|100|100|100", 98)]

    [InlineData("0|0|100", 50)]
    [InlineData("0|0|100|100", 70)]
    [InlineData("0|0|100|100|100", 80)]
    [InlineData("0|0|100|100|100|100", 86)]
    [InlineData("0|0|100|100|100|100|100", 89)]
    [InlineData("0|0|100|100|100|100|100|100", 92)]
    [InlineData("0|0|100|100|100|100|100|100|100", 93)]
    [InlineData("0|0|100|100|100|100|100|100|100|100", 95)]
    [InlineData("0|0|100|100|100|100|100|100|100|100|100", 95)]
    [InlineData("0|0|100|100|100|100|100|100|100|100|100|100", 96)]
    [InlineData("0|0|100|100|100|100|100|100|100|100|100|100|100", 97)]

    [InlineData("0|0|0|100|100", 60)]
    [InlineData("0|0|0|100|100|100", 71)]
    [InlineData("0|0|0|100|100|100|100", 79)]
    [InlineData("0|0|0|100|100|100|100|100", 83)]
    [InlineData("0|0|0|100|100|100|100|100|100", 87)]
    [InlineData("0|0|0|100|100|100|100|100|100|100", 89)]
    [InlineData("0|0|0|100|100|100|100|100|100|100|100", 91)]
    [InlineData("0|0|0|100|100|100|100|100|100|100|100|100", 92)]
    [InlineData("0|0|0|100|100|100|100|100|100|100|100|100|100", 93)]
    [InlineData("0|0|0|100|100|100|100|100|100|100|100|100|100|100", 94)]
    [InlineData("0|0|0|100|100|100|100|100|100|100|100|100|100|100|100", 95)]

    [InlineData("100|0", 33)]
    [InlineData("100|100|0", 50)]
    [InlineData("100|100|100|0", 60)]
    [InlineData("100|100|100|100|0", 67)]
    [InlineData("100|100|100|100|100|0", 71)]
    [InlineData("100|100|100|100|100|100|0", 75)]
    [InlineData("100|100|100|100|100|100|100|0", 78)]
    [InlineData("100|100|100|100|100|100|100|100|0", 80)]
    public void Test(string answerHistory, int expectedCalculatedAverage)
    {
        var answerScores = answerHistory.Split('|', StringSplitOptions.RemoveEmptyEntries).Select(decimal.Parse).ToArray();
        Assert.Equal(expectedCalculatedAverage, Math.Round(AscendingWeightedAverage.Of(answerScores)));
    }
}