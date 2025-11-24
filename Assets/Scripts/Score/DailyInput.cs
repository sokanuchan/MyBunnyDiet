using System.Collections.Generic;

public class DailyInput
{
    public enum Mood
    {
        None,
        VerySad,
        Sad,
        Medium,
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
    Mood mood = Mood.None;

    public static DailyInput currentDailyInput = new DailyInput();
}
