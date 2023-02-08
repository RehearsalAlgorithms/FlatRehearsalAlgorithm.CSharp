namespace FlatRehearsalAlgorithm;

public class LearnItemPicker<TLearnItem> where TLearnItem : ILearnItem
{
    /// <summary>
    /// The ultimate goal is to reach at least this score for all items (see <see cref="ILearnItem.Score"/>).<br/>
    /// When all items are added to <see cref="LearnItemsForThisSession"/>, the <see cref="LearnItemPicker{TLearnItem}"/> uses this goal score.
    /// </summary>
    protected const int GoalScoreForAllItems = 80;
    /// <summary>
    /// If all items in the list <see cref="LearnItemsForThisSession"/> have at least this score, a new set of new or hard items is added via <see cref="AddNewOrHardItemsToThisSession"/>.
    /// The number of new or hard items depends on <see cref="LearnItemPickerConfig.NewOrHardItemsPerSession"/>.
    /// </summary>
    protected const int GoalScoreForThisSession = 70;
    /// <summary>
    /// If more than <see cref="LearnItemPickerConfig.LearnInGroupsOf"/> have a score lower than this (including the zero score in the case of new items) those items are studied in isolation of group size <see cref="LearnItemPickerConfig.LearnInGroupsOf"/>.
    /// </summary>
    protected const int GoalScoreForIntensiveStudy = 60;

    /// <summary>
    /// Call <see cref="NextLearnItem"/> to load another item.
    /// </summary>
    public TLearnItem CurrentLearnItem { get; protected set; }

    /// <summary>
    /// If you want to make learning harder for yourself, then choose greater numbers for the settings <see cref="LearnItemPickerConfig.NewOrHardItemsPerSession"/> and <see cref="LearnItemPickerConfig.LearnInGroupsOf"/>.<br/>
    /// Lower numbers may result in consuming less brain energy, but it might feel more boring because there is more repitition.<br/>
    /// Higher numbers might speed up the learning process because you see more different items in the same time, but beware that it might feel exhausting and that retention might decrease.
    /// </summary>
    public LearnItemPickerConfig Config { get; }
    protected List<TLearnItem> AllLearnItems { get; }
    protected List<TLearnItem> LearnItemsForThisSession { get; } = new();
    protected List<TLearnItem> LearnItemsForIntensiveStudy { get; } = new();
    protected Queue<TLearnItem> RecentlyAskedLearnItems { get; } = new();
    public LearnItemPicker(List<TLearnItem> allLearnItems, LearnItemPickerConfig? config = null)
    {
        const int minLearnItemCount = 5;
        if (allLearnItems.Count < minLearnItemCount)
        {
            throw new ArgumentException($"The number of items should be at least {minLearnItemCount}.", nameof(allLearnItems));
        }
        AllLearnItems = allLearnItems;
        Config = config ?? new();
        AddNewOrHardItemsToThisSession();
        CurrentLearnItem = PickAnItemThatHasNotBeenAskedRecently();
    }

    protected void AddNewOrHardItemsToThisSession()
    {
        LearnItemsForIntensiveStudy.Clear();
        LearnItemsForThisSession.Clear();

        var unfinishedOrNew = AllLearnItems.OrderDeferredForGoalScore(GoalScoreForAllItems).Take(Config.NewOrHardItemsPerSession);
        var allStudiedItems = AllLearnItems.Where(x => x.HasBeenStudied());
        LearnItemsForThisSession.AddRange(unfinishedOrNew.Union(allStudiedItems));
    }

    /// <summary>
    /// Sets a new value for <see cref="CurrentLearnItem"/>.
    /// </summary>
    /// <returns><see cref="CurrentLearnItem"/></returns>
    public TLearnItem NextLearnItem()
    {
        SwitchStudyModeIfNecessary();
        CurrentLearnItem = PickAnItemThatHasNotBeenAskedRecently();
        return CurrentLearnItem;
    }

    protected List<TLearnItem> CurrentList => LearnItemsForIntensiveStudy.Count > 0 ? LearnItemsForIntensiveStudy : LearnItemsForThisSession;

    protected int GoalForCurrentList()
    {
        if (CurrentList == LearnItemsForIntensiveStudy) return GoalScoreForIntensiveStudy;
        else if (LearnItemsForThisSession.Count == AllLearnItems.Count) return GoalScoreForAllItems;
        else if (CurrentList == LearnItemsForThisSession) return GoalScoreForThisSession;
        throw new ApplicationException($"{nameof(GoalForCurrentList)} bug: it does not recognize the {nameof(CurrentList)}.");
    }

    protected int WaitBetweenIntensiveStudies => Config.LearnInGroupsOf / 2;
    protected int waitCountSinceLastIntensiveStudy = 0;
    protected void SwitchStudyModeIfNecessary()
    {
        if (CurrentList == LearnItemsForIntensiveStudy)
        {
            LearnItemsForIntensiveStudy.RemoveAll(item => item.Score >= GoalScoreForAllItems);

            if (LearnItemsForIntensiveStudy.Count < 3 || LearnItemsForIntensiveStudy.All(item => item.Score >= GoalScoreForIntensiveStudy))
            {
                LearnItemsForIntensiveStudy.Clear();
            }
        }

        if (CurrentList != LearnItemsForIntensiveStudy
            && CurrentList.Where(item => item.Score < GoalScoreForIntensiveStudy).MoreThan(Config.LearnInGroupsOf)
            && waitCountSinceLastIntensiveStudy++ == WaitBetweenIntensiveStudies)
        {
            waitCountSinceLastIntensiveStudy = 0;
            var nextItemsToStudyIntensively = CurrentList.OrderDeferredForGoalScore(GoalScoreForIntensiveStudy).Take(Math.Min(Config.LearnInGroupsOf, LearnItemsForThisSession.Count));
            LearnItemsForIntensiveStudy.AddRange(nextItemsToStudyIntensively);
        }
        else if (LearnItemsForThisSession.Count != AllLearnItems.Count && LearnItemsForThisSession.All(item => item.Score >= GoalScoreForThisSession))
        {
            AddNewOrHardItemsToThisSession();
        }
    }

    protected TLearnItem PickAnItemThatHasNotBeenAskedRecently()
    {
        var minimumNumberOfTimesNotAsked = Math.Min(10, CurrentList.Count / 2);
        while (RecentlyAskedLearnItems.Count > minimumNumberOfTimesNotAsked)
        {
            RecentlyAskedLearnItems.Dequeue();
        }

        var notRecentlyAsked = CurrentList.Where(i => !RecentlyAskedLearnItems.Contains(i)).ToList();
        var item = InSessionPicker.Pick(notRecentlyAsked, GoalForCurrentList());

        RecentlyAskedLearnItems.Enqueue(item);
        return item;
    }
}
