using System.Collections.Generic;
using System.Runtime.Serialization;

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

    // Mini game
    [DataMember(IsRequired = false)]
    public int bunnyStars;
    [DataMember(IsRequired = false)]
    public List<string> boughtItems;
}
