using System.Collections.Generic;

public class DailyInput
{
    public enum Mood
    {
        None,
        VerySad,
        Sad,
        Neutral,
        Happy,
        VeryHappy,
    }

    // calories
    public int calories = 0;

    // sport
    public int muscu = 0;
    public int walk = 0;
    public int cardio = 0;

    // mood
    public Mood mood = Mood.None;

    // computed score
    public int score = 0;

    // current player input
    public static DailyInput currentDailyInput = new DailyInput();
    public static string currentDate;

    // all player inputs
    public static SortedDictionary<string, DailyInput> playerInputs = new SortedDictionary<string, DailyInput>();
}
