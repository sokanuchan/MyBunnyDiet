using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static int totalScore = 0;
    private static int currentScore;
    public static int nbBunnyParts = 0;
    public static int startingWeight = 127;
    public static int height = 158;
    public static int age = 21;
    public static float muscuRatio = 17;
    public static float walkRatio = 1f / 10;
    public static float cardioRatio = 9;


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

    public static int GetCaloriesGoal(DateTime date)
    {
        // compute current weight (1kg should be lost per about 8000 score)
        int currentWeight = startingWeight - (GetTotalScoreAtCurrentDay(date) / 8000);

        for (currentWeight = 1; currentWeight < startingWeight; currentWeight++)
        {
            // compute base metabolism
            float weightMetabolism = currentWeight * 10;
            float heightMetabolism = height * 6.25f;
            float ageMetabolism = 5 * age;
            float constantMetabolism = 161;
            float metabolismRatio = 1.3f;
            float metabolism = weightMetabolism + heightMetabolism - ageMetabolism - constantMetabolism;


            Debug.Log(currentWeight.ToString() + ": " + (int)(metabolism * metabolismRatio));
        }

        // compute base metabolism

        // return complete metabolism
        return 1;
    }

    private static int GetTotalScoreAtCurrentDay(DateTime date)
    {
        int score = 0;

        // add up all scores until specified date
        foreach (string playerInputDate in DailyInput.playerInputs.Keys)
        {
            // stop adding up scores if we are past the specified date
            if (date < DateTime.ParseExact(playerInputDate, DateUtils.dailyInputDateFormat, null))
            {
                break;
            }

            score += DailyInput.playerInputs[playerInputDate].score;
        }

        return score;
    }

    public static void ChangeCaloriesScore(ScoreChanges scoreChanges, int calories, DateTime date)
    {
        int tmpScoreChange = 0;
        int caloriesPerDayGoal = GetCaloriesGoal(date);

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

    public static ScoreChanges GetScoreChanges(DailyInput dailyInput, DateTime date)
    {
        ScoreChanges scoreChanges = new ScoreChanges();

        // calories changes
        ChangeCaloriesScore(scoreChanges, dailyInput.calories, date);

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
