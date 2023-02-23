## FlatRehearsalAlgorithm for C#

FlatRehearsalAlgorithm for C# provides an algorithm for rote learning - memorization of information based on repetition. It is designed for both a small and a great number of pieces of information (let's say 20 items for kids and over a 1000 items for adults). This rehearsal algorithm is called "flat" because no learn item should depend on a specific other learn item before it can be studied - the algorithm treats all learn items as independent. Note however that this algorithm can be used for mastering "layered topics" as well: one set of items (let's say "addition until 10") could be studied first and when the score for that topic is high enough, another topic (let's say "addition until 30") can be studied next by then feeding those other items to the algorithm. Also note that the learn items don't need to be static like lists of words: you can also design learn items that allow generating slightly different content on-the-fly.

> This library is in pre-release and still needs to be tested and fine tuned.

### An execution timeline

1. You or the user of your app provides a list of "learn items" - those can be words from one language to another, names and faces, simple questions about topographic maps, etc.
2. For each of those items, you instantiate a `LearnItem` - which inherits the interface `ILearnItem` from this package and you feed all those items to the `LearnItemPicker`. Immediately a first `CurrentLearnItem` has been picked that you can display to the user in whatever way fits your app.
3. When the user has answered, you check whether the user has answered correctly (`100` points), incorrectly (`0` points) or you may choose something in between and then you call `learnItem.ProcessNewAnswerScore(score)` - this method sets all properties that the `ILearnItem` interface has: `LastTimeAskedUtcTicks`, `ScoreHistory` and `Score`. No other interaction with the `ILearnItem` interface is needed. You could also give the user the option not to process the answer in case of a typo - such things are up to you. Then you call `learnItemPicker.NextLearnItem()` to load another learn item as `CurrentLearnItem`.
4. You can repeat step 3 many times. Because you called `learnItem.ProcessNewAnswerScore(score)` many times now, the `LearnItemPicker` will be able to sort and pick items for the user intelligently: if an item has a low score, it will be repeated more often but not too often. Items that seem to be mastered, will almost never be asked but won't be forgotten.
5. Gradually, more learn items are added to the pool of items that the user studies.
6. ...until all items are mastered.


## How to use this library

In [Visual Studio](https://visualstudio.microsoft.com/downloads/)'s `Solution Explorer`, right-click a project and click `Manage NuGet Packages...`. Browse and install "FlatRehearsalAlgorithm".

Add
```csharp
using FlatRehearsalAlgorithm;
```

Then you can declare a class that implements `ILearnItem`:
```csharp
public class MyLearnItem : ILearnItem
{
    public anything Id { get; set; }
    public anything DummyQuestion { get; set; } // An image? A text with variable parts?
    public anything DummyCorrectAnswers { get; set; }

    // only the properties below are required:
	public long? LastTimeAskedUtcTicks { get; set; }
    public int Score { get; set; }
    public string? ScoreHistory { get; set; }
}
```
You can instantiate this class for each item for a one-time rote learn session, but you probably want to store the progress in a database. In the latter case, use `varchar(200)` for `ScoreHistory`.

Now you have `items` and you can feed them to the `LearnItemPicker` and play with it:
```csharp
var learnItemPicker = new LearnItemPicker<MyLearnItem>(items);
var firstItem = learnItemPicker.CurrentLearnItem;
// display first item to the user, get the user's answer, provide feedback, etc.
firstItem.ProcessNewAnswerScore(0);
var secondItem = learnItemPicker.NextLearnItem();
// display second item to the user, get the user's answer, provide feedback, etc.
secondLearnItem.ProcessNewAnswerScore(100);
var thirdItem = learnItemPicker.NextLearnItem();
// etc.
```

## License

This repository uses the most permissive licensing available. The "BSD Zero Clause License" ([0BSD](https://choosealicense.com/licenses/0bsd/)) allows for<br/>
commercial + non-commercial use, closed + open source, with + without modifications, etc. and [is equivalent](https://github.com/github/choosealicense.com/issues/805) to licenses like:

- "MIT No Attribution License" ([MIT-0](https://choosealicense.com/licenses/mit-0//)).
- "The Unlicense" ([Unlicense](https://choosealicense.com/licenses/unlicense/)).
- "CC0" ([CC0](https://choosealicense.com/licenses/cc0/)).

The "BSD Zero Clause License" ([0BSD](https://choosealicense.com/licenses/0bsd/)) does not have the condition

> (...), provided that the above copyright notice and this permission notice appear in all copies.

which is part of the "MIT License" ([MIT](https://choosealicense.com/licenses/mit/)) and its shorter equivalent "ISC License" ([ISC](https://choosealicense.com/licenses/isc/)). Apart from that they are all equivalent.


## Ask or contribute

- [ask questions](https://github.com/RehearsalAlgorithms/FlatRehearsalAlgorithm.CSharp/discussions) about anything that is not clear or when you'd like help.
- [share](https://github.com/RehearsalAlgorithms/FlatRehearsalAlgorithm.CSharp/discussions) ideas or what you've made.
- [report a bug](https://github.com/RehearsalAlgorithms/FlatRehearsalAlgorithm.CSharp/issues).
- [request an enhancement](https://github.com/RehearsalAlgorithms/FlatRehearsalAlgorithm.CSharp/issues).
- [open a pull request](https://github.com/RehearsalAlgorithms/FlatRehearsalAlgorithm.CSharp/pulls). 
