namespace FlatRehearsalAlgorithm;

public interface ILearnItem
{
    /// <summary>
    /// Is set by <see cref="ILearnItemExtensions.ProcessNewAnswerScore"/>.
    /// </summary>
    public long? LastTimeAskedUtcTicks { get; set; }

    /// <summary>
    /// Is set by <see cref="ILearnItemExtensions.ProcessNewAnswerScore"/>.<br/>
    /// Range: [0, 100].
    /// </summary>
    public int Score { get; set; }

    /// <summary>
    /// Is set by <see cref="ILearnItemExtensions.ProcessNewAnswerScore"/>.<br/>
    /// May look like <c>"0|100|0|100|100"</c>.<br/>
    /// If you want to store this in a database, you can use the type <c>varchar(200)</c> (because only the latest 50 answer scores are kept and each take a maximum of 4 characters - including the separator).
    /// </summary>
    public string? ScoreHistory { get; set; }
}

public static class ILearnItemExtensions
{
    public static bool HasBeenStudied(this ILearnItem x)
    {
        return x.LastTimeAskedUtcTicks is not null;
    }

    /// <summary>
    /// The <see cref="ILearnItem.Score"/> property should be recalculated via this method each time a user has answered.<br/>
    /// If a user has answered a LearnItem three times, the first time with 100 score, the second time with 100 score and then 0 score,<br/>
    /// then the calculated <see cref="ILearnItem.Score"/> property becomes (1 * 100 + 2 * 100 + 3 * 0) / (1 + 2 + 3) = 300 / 6 = 50.<br/>
    /// Thus more recently given answers weigh heavier.
    /// </summary>
    /// <param name="item">The <see cref="ILearnItem"/> that the user has answered.</param>
    /// <param name="lastAnswerScore">Incorrect = 0. Correct = 100. For partially correct answers, you could choose something in between.</param>
    public static TLearnItem ProcessNewAnswerScore<TLearnItem>(this TLearnItem item, int lastAnswerScore) where TLearnItem : ILearnItem
    {
        if (lastAnswerScore < 0 || lastAnswerScore > 100)
        {
            throw new ArgumentException($"The {nameof(lastAnswerScore)} should be 0 or 100 or something in between.");
        }
        var scores = (item.ScoreHistory ?? "")
            .Split("|", StringSplitOptions.RemoveEmptyEntries).TakeLast(49).Select(decimal.Parse)
            .Concat(new[] { (decimal)lastAnswerScore }).ToArray();
        item.Score = (int)Math.Round(AscendingWeightedAverage.Of(scores));
        item.ScoreHistory = string.Join("|", scores.Select(x => (int)x));
        item.LastTimeAskedUtcTicks = DateTime.UtcNow.Ticks;
        return item;
    }
}