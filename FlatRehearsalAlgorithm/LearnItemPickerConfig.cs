namespace FlatRehearsalAlgorithm;

/// <summary>
/// If you want to make learning harder for yourself, then choose greater numbers for the settings <see cref="NewOrHardItemsPerSession"/> and <see cref="LearnInGroupsOf"/>.<br/>
/// Lower numbers may result in consuming less brain energy, but it might feel more boring because there is more repitition.<br/>
/// Higher numbers might speed up the learning process because you see more different items in the same time, but beware that it might feel exhausting and that retention might decrease.
/// </summary>
public class LearnItemPickerConfig
{
    /// <summary>
    /// A new learning session will use:<br/>
    /// - the hard* items of the previous learning session.<br/>
    /// - new items.<br/>
    /// - mastered items for rehearsal.<br/>
    /// How many new + hard items would you like to master per (hourly) learning session?<br/>
    /// Note 1: as soon as all the new and hard items are mastered, new items are added to the session automatically. (It does not depend on time.)
    /// Note 2: you don't have to change this setting if your total of items to learn is less.
    /// </summary>
    public int NewOrHardItemsPerSession
    {
        get => _newOrHardItemsPerSession;
        set
        {
            const int minValue = 20;
            const int maxValue = 60;
            if (value < minValue)
            {
                throw new Exception($"The value of {nameof(NewOrHardItemsPerSession)} should be at least {minValue}.");
            }
            else if (value <= LearnInGroupsOf)
            {
                throw new Exception($"The value of {nameof(NewOrHardItemsPerSession)} should be greater than the value of {nameof(LearnInGroupsOf)}.");
            }
            else if (value > maxValue)
            {
                throw new Exception($"The value of {nameof(NewOrHardItemsPerSession)} should not be greater than {maxValue}. Note that as soon as the new and hard items are mastered, a new session is loaded automatically.");
            }
        }
    }
    private int _newOrHardItemsPerSession = Defaults.NewOrHardItemsPerSession;

    /// <summary>
    /// Completely new items are studied in isolated groups until the <see cref="ILearnItem.Score"/> of the items equals <see cref="LearnItemPicker.GoalScoreForIntensiveStudy"/>.<br/>
    /// Items with low scores are sometimes also isolated into groups of this size to study intensively.
    /// </summary>
    public int LearnInGroupsOf
    { 
        get => _learnInGroupsOf;
        set 
        {
            const int minValue = 6;
            if (value < minValue)
            {
                throw new Exception($"The value of {nameof(LearnInGroupsOf)} should be at least {minValue}.");
            }
            else if (value >= NewOrHardItemsPerSession)
            {
                throw new Exception($"The value of {nameof(LearnInGroupsOf)} should be smaller than the value of {nameof(NewOrHardItemsPerSession)}.");
            }
        }
    }
    private int _learnInGroupsOf = Defaults.LearnInGroupsOf;

    public static class Defaults
    {
        public static readonly int NewOrHardItemsPerSession = 30;
        public static readonly int LearnInGroupsOf = 6;
    }
}
