using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static int caloriesPerDayGoal = 2300;
    public static int totalScore = 0;
    public static int nbBunnyParts = 0;
    public static float muscuRatio = 17;
    public static float walkRatio = 1f/10;
    public static float cardioRatio = 9;

    private static int currentScore;

    // constants
    private static int caloriesPerDayMargin = 300;
    private static int tooLowCaloriesScoreRatio = 5;

    public class ScoreChanges
    {
        public List<string> positiveChanges = new List<string>();
        public List<string> negativeChanges = new List<string>();
        public int totalChanges = 0;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void ChangeCaloriesScore(ScoreChanges scoreChanges, int calories)
    {
        int tmpScoreChange = 0;

        // calories below minimum
        if (calories < (caloriesPerDayGoal - caloriesPerDayMargin))
        {
            tmpScoreChange = (calories + caloriesPerDayMargin - caloriesPerDayGoal) / tooLowCaloriesScoreRatio;
            scoreChanges.totalChanges += tmpScoreChange;
            scoreChanges.negativeChanges.Add(tmpScoreChange.ToString() + " " + "Calories en dessous du seuil minimum");
        }

        // compare calories to calories goal
        tmpScoreChange = caloriesPerDayGoal - calories;
        scoreChanges.totalChanges += tmpScoreChange;
        if (tmpScoreChange < 0) // too many calories
        {
            scoreChanges.negativeChanges.Add(tmpScoreChange.ToString() + " " + "Surplus calorique");
        }
        else // calorie deficit
        {
            scoreChanges.positiveChanges.Add(tmpScoreChange.ToString() + " " + "Deficit calorique");
        }
    }

    public static void ChangeSportScore(ScoreChanges scoreChanges, float ratioToUse, string sportNameToDisplay, int value)
    {
        if (value == 0)
        {  
            return; 
        }

        int tmpScoreChange = (int) (ratioToUse * value);
        scoreChanges.totalChanges += tmpScoreChange;
        scoreChanges.positiveChanges.Add(tmpScoreChange.ToString() + " " + sportNameToDisplay);
    }

    public static ScoreChanges GetScoreChanges(DailyInput dailyInput)
    {
        ScoreChanges scoreChanges = new ScoreChanges();

        // calories changes
        ChangeCaloriesScore(scoreChanges, dailyInput.calories);

        // sport changes
        ChangeSportScore(scoreChanges, muscuRatio, "Musculation", dailyInput.muscu);
        ChangeSportScore(scoreChanges, walkRatio, "Marche", dailyInput.walk);
        ChangeSportScore(scoreChanges, cardioRatio, "Cardio", dailyInput.cardio);

        // update currentScore
        currentScore = scoreChanges.totalChanges;

        return scoreChanges;
    }

    public static void UpdateScore()
    {
        // get current date
        string currentDate = DailyInput.currentDate;

        // get old score
        int oldScore = 0;
        if (DailyInput.playerInputs.ContainsKey(currentDate))
        {
            oldScore = DailyInput.playerInputs[currentDate].score;
        }

        // get new score
        int newScore = currentScore;

        // update total score
        int scoreDiff = newScore - oldScore;
        totalScore += scoreDiff;

        // update bunny parts
        nbBunnyParts = Mathf.Max(totalScore / 1000, nbBunnyParts);

        // update player input
        DailyInput.currentDailyInput.score = newScore;
        DailyInput.playerInputs[currentDate] = DailyInput.currentDailyInput;
        SaveManager.Save();
    }
}
