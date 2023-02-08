namespace FlatRehearsalAlgorithm.Tests;

public class LearnItemPickerTests
{
    [Fact]
    public void CurrentLearnItem_is_loaded_after_initialization()
    {
        var items = Enumerable.Range(1, 9).Select(x => new DummyLearnItem { DummyId = x.ToString() }).ToList();
        var learnItemPicker = new LearnItemPicker<DummyLearnItem>(items);
        Assert.NotNull(learnItemPicker.CurrentLearnItem);
        Assert.Contains(learnItemPicker.CurrentLearnItem, items);
    }

    [Fact]
    public void Calling_NextLearnItem_sets_another_CurrentLearnItem()
    {
        // Arrange
        var items = Enumerable.Range(1, 9).Select(x => new DummyLearnItem { DummyId = x.ToString() }).ToList();
        var learnItemPicker = new LearnItemPicker<DummyLearnItem>(items);
        var first = learnItemPicker.CurrentLearnItem;

        // Act & assert 1
        var second = learnItemPicker.NextLearnItem();
        Assert.NotEqual(first, learnItemPicker.CurrentLearnItem);
        Assert.Equal(second, learnItemPicker.CurrentLearnItem);

        // Act & assert 2
        var third = learnItemPicker.NextLearnItem();
        Assert.NotEqual(first, learnItemPicker.CurrentLearnItem);
        Assert.NotEqual(second, learnItemPicker.CurrentLearnItem);
        Assert.Equal(third, learnItemPicker.CurrentLearnItem);
    }

    [Fact]
    public void Eventually_the_LearnItemPicker_will_have_picked_all_items()
    {
        var pickedItems = new HashSet<string>();
        var items = Enumerable.Range(1, 80).Select(x => new DummyLearnItem { DummyId = x.ToString() }).ToList();
        var learnItemPicker = new LearnItemPicker<DummyLearnItem>(items);
        
        for (var i = 0; i < 200; i++)
        {
            learnItemPicker.CurrentLearnItem.ProcessNewAnswerScore(90);
            pickedItems.Add(learnItemPicker.NextLearnItem().DummyId);
        }

        Assert.Equal(items.Count, pickedItems.Count);
    }
}
