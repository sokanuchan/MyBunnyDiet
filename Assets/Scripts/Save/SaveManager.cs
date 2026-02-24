using System.Collections.Generic;
using System.Diagnostics;

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

        // save mini game data
        SaveData.current.bunnyStars = ShopManager.bunnyStars;
        SaveData.current.boughtItems = BoughtItemsManager.boughtItems;

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

        // load mini game data
        ShopManager.bunnyStars = SaveData.current.bunnyStars;
        if (SaveData.current.boughtItems != null)
        {
            BoughtItemsManager.boughtItems = SaveData.current.boughtItems;
        }
        else
        {
            BoughtItemsManager.boughtItems = new List<string> ();
        }
    }

    public static void ResetSave()
    {
        DailyInput.playerInputs = new SortedDictionary<string, DailyInput>();
        BunniesMenu.unlockedBunnies = new List<int>();
        ScoreManager.nbBunnyParts = 0;
        ScoreManager.totalScore = 0;
        ShopManager.bunnyStars = 0;
        BoughtItemsManager.boughtItems = new List<string>();

        Save();
    }
}
