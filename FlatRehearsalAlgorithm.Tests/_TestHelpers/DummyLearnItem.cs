namespace FlatRehearsalAlgorithm.Tests._TestHelpers;

public class DummyLearnItem : ILearnItem
{
    public string DummyId { get; set; } = null!;
    public long? LastTimeAskedUtcTicks { get; set; }
    public int Score { get; set; }
    public string? ScoreHistory { get; set; }
}