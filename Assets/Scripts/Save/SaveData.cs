using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    private static SaveData _current;
    public static SaveData current
    {
        get
        {
            if (_current == null)
            {
                _current = new SaveData();
            }

            return _current;
        }
        set
        {
            _current = value;
        }
    }

    // player inputs
    public SortedDictionary<string, DailyInput> playerInputs;

    // Bunnies
    public List<int> unlockedBunnies;
    public int nbBunnyParts;
    public int totalScore;
}
