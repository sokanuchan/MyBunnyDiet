using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static int caloriesPerDayGoal = 2300;
    public struct ScoreChanges
    {
        public List<string> positiveChanges;
        public List<string> negativeChanges;
        public int totalChanges;

        public ScoreChanges (List<string> positiveChanges, List<string> negativeChanges, int totalChanges)
        {
            this.positiveChanges = positiveChanges;
            this.negativeChanges = negativeChanges;
            this.totalChanges = totalChanges;
        }
    }
    public static ScoreChanges currentScoreChanges = new ScoreChanges(new List<string>(), new List<string>(), 0);

    private static int caloriesPerDayMargin = 300;
    private static int tooLowCaloriesScoreRatio = 5;
    private static float muscuRatio = 17;
    private static float walkRatio = 1f/10;
    private static float cardioRatio = 9;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void ChangeCalories(int calories)
    {
        int tmpScoreChange = 0;

        // calories below minimum
        if (calories < (caloriesPerDayGoal - caloriesPerDayMargin))
        {
            tmpScoreChange = (calories + caloriesPerDayMargin - caloriesPerDayGoal) / tooLowCaloriesScoreRatio;
            currentScoreChanges.totalChanges += tmpScoreChange;
            currentScoreChanges.negativeChanges.Add(tmpScoreChange.ToString() + " " + "Calories en dessous du seuil minimum");
        }

        // compare calories to calories goal
        tmpScoreChange = caloriesPerDayGoal - calories;
        currentScoreChanges.totalChanges += tmpScoreChange;
        if (tmpScoreChange < 0) // too many calories
        {
            currentScoreChanges.negativeChanges.Add(tmpScoreChange.ToString() + " " + "Surplus calorique");
        }
        else // calorie deficit
        {
            currentScoreChanges.positiveChanges.Add(tmpScoreChange.ToString() + " " + "Deficit calorique");
        }
    }

    public static void ChangeSport(string sportName, int value)
    {
        float ratio = 0;
        string sportNameToDisplay = "";

        switch (sportName)
        {
            case "Muscu":
                ratio = muscuRatio;
                sportNameToDisplay = "Musculation";
                break;
            case "Walk":
                sportNameToDisplay = "Marche";
                ratio = walkRatio;
                break;
            case "Cardio":
                sportNameToDisplay = "Cardio";
                ratio = cardioRatio;
                break;
        }

        Debug.Log(ratio);
        int tmpScoreChange = (int) (ratio * value);
        currentScoreChanges.totalChanges += tmpScoreChange;
        currentScoreChanges.positiveChanges.Add(tmpScoreChange.ToString() + " " + sportNameToDisplay);
    }
}
