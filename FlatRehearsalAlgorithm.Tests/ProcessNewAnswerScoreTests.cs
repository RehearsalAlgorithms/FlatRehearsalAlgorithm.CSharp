namespace FlatRehearsalAlgorithm.Tests;

public class ProcessNewAnswerScoreTests
{
    [Fact]
    public void LessThan0ShouldThrow()
    {
        var x = new DummyLearnItem();
        var exception = Assert.Throws<ArgumentException>(() => x.ProcessNewAnswerScore(-1));
        Assert.Equal("The lastAnswerScore should be 0 or 100 or something in between.", exception.Message);
    }

    [Fact]
    public void GreaterThan100ShouldThrow()
    {
        var item = new DummyLearnItem();
        var exception = Assert.Throws<ArgumentException>(() => item.ProcessNewAnswerScore(101));
        Assert.Equal("The lastAnswerScore should be 0 or 100 or something in between.", exception.Message);
    }

    /// <summary>
    /// Also see <see cref="Internals.AscendingWeightedAverageTests"/>.
    /// </summary>
    [Fact]
    public void SetsMultipleProperties()
    {
        // Arrange
        var x = new DummyLearnItem();
        Assert.Null(x.LastTimeAskedUtcTicks);
        Assert.Null(x.ScoreHistory);

        // Act & assert 1
        x.ProcessNewAnswerScore(0);
        Assert.NotNull(x.LastTimeAskedUtcTicks);
        Assert.Equal("0", x.ScoreHistory);
        Assert.Equal(0, x.Score);

        // Act & assert 2
        x.ProcessNewAnswerScore(100);
        Assert.Equal("0|100", x.ScoreHistory);
        Assert.Equal(67, x.Score);
    }

    [Fact]
    public void KeepsAHistoryOfLast50AnswerScores()
    {
        var item = new DummyLearnItem();
        for(var i = 1; i <= 50; i++)
        {
            item.ProcessNewAnswerScore(i);
        }
        Assert.Equal("1|2|3|4|5|6|7|8|9|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31|32|33|34|35|36|37|38|39|40|41|42|43|44|45|46|47|48|49|50", item.ScoreHistory);
        item.ProcessNewAnswerScore(51);
        Assert.Equal("2|3|4|5|6|7|8|9|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31|32|33|34|35|36|37|38|39|40|41|42|43|44|45|46|47|48|49|50|51", item.ScoreHistory);
    }
}
