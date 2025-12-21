using System.Collections.Generic;

public class SaveManager
{
    public static void Save()
    {
        // save playerInputs
        SaveData.current.playerInputs = DailyInput.playerInputs;

        // save bunnies
        SaveData.current.unlockedBunnies = BunniesMenu.unlockedBunnies;
        SaveData.current.nbBunnyParts = ScoreManager.nbBunnyParts;
        SaveData.current.totalScore = ScoreManager.totalScore;

        // save file
        SerializationManager.Save("save_file", SaveData.current);
    }

    public static void Load()
    {
        // load file
        SaveData.current = (SaveData)SerializationManager.Load("save_file");

        // load playerInputs
        DailyInput.playerInputs = SaveData.current.playerInputs;

        // load bunnies
        BunniesMenu.unlockedBunnies = SaveData.current.unlockedBunnies;
        ScoreManager.nbBunnyParts = SaveData.current.nbBunnyParts;
        ScoreManager.totalScore = SaveData.current.totalScore;
    }

    public static void ResetSave()
    {
        DailyInput.playerInputs = new SortedDictionary<string, DailyInput>();
        BunniesMenu.unlockedBunnies = new List<int>();
        ScoreManager.nbBunnyParts = 0;
        ScoreManager.totalScore = 0;

        Save();
    }
}
