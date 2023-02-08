namespace FlatRehearsalAlgorithm.Tests.Internals;

public class OrderForGoalScoreTests
{
    [Fact]
    public void OrderedViaProperties()
    {
        // Arrange
        var itemList = new List<DummyLearnItem>
        {
            new DummyLearnItem
            {
                DummyId = "New",
            },
            new DummyLearnItem
            {
                DummyId = "Perfect score",
            }.ProcessNewAnswerScore(100),
            new DummyLearnItem
            {
                DummyId = "Worst score",
            }.ProcessNewAnswerScore(0),
            new DummyLearnItem
            {
                DummyId = "Half score recently",
                Score = 50,
                LastTimeAskedUtcTicks = 20,
            },
            new DummyLearnItem
            {
                DummyId = "Half score long ago",
                Score = 50,
                LastTimeAskedUtcTicks = 10,
            },
        };

        // Act
        itemList.SortForGoalScore(80);

        // Assert
        Assert.Equal(new[]
        {
            "Worst score",
            "Half score long ago",
            "Half score recently",
            "New",
            "Perfect score"
        }, itemList.Select(x => x.DummyId));
    }

    [Fact]
    public void RandomIfSortingPropertiesAreEqual()
    {
        // Arrange
        var itemList = Enumerable.Range(1, 20).Select(x => new DummyLearnItem
            {
                DummyId = "New " + x,
            })
            .Concat(Enumerable.Range(21, 20).Select(x => new DummyLearnItem
            {
                DummyId = "Studied badly and equal to other studied items " + x,
                Score = 50,
                LastTimeAskedUtcTicks = 50,
            })).ToList();

        Assert.Equal(40, itemList.Count);

        // Act & assert
        var ordered = itemList.OrderDeferredForGoalScore(80).ToList();
        Assert.True(ordered.Take(20).All(x => x.DummyId.StartsWith("Studied badly")));
        Assert.True(ordered.TakeLast(20).All(x => x.DummyId.StartsWith("New")));

        var other = itemList.OrderDeferredForGoalScore(80).ToList();
        Assert.True(other.Take(20).All(x => x.DummyId.StartsWith("Studied badly")));
        Assert.True(other.TakeLast(20).All(x => x.DummyId.StartsWith("New")));

        Assert.NotEqual(ordered.Take(20), other.Take(20));
        Assert.NotEqual(ordered.TakeLast(20), other.TakeLast(20));
    }
}
