using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DailyCategoriesMenu : MonoBehaviour
{
    public Text date;
    public Text currentCalories;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // grey the Validate button while calories have not been input
        if (DailyInput.currentDailyInput.calories == 0)
        {
            GameObject.Find("Validate").GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
        }

        // update date with current input date
        date.text = DateUtils.ChangeDateFormat(DailyInput.currentDate, DateUtils.dailyInputDateFormat, DateUtils.playerDisplayDateFormat);

        // update current calories with current input calories
        currentCalories.text = "Current: " + DailyInput.currentDailyInput.calories + "\nExpected: " + ScoreManager.GetCaloriesGoal(DateTime.ParseExact(DailyInput.currentDate, DateUtils.dailyInputDateFormat, null)).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        // get hit button
        string hitButton = MenuManager.GetHitButton();

        // handle hit button
        switch (hitButton)
        {
            case "Calories":
                KeyboardManager.GetInput("Combien de calories aujourd'hui ?", hitButton);
                break;
            case "Sport":
                SceneManager.LoadScene("SportInputs");
                break;
            case "Mood":
                SceneManager.LoadScene("Mood");
                break;
            case "Validate":
                if (DailyInput.currentDailyInput.calories != 0)
                {
                    SceneManager.LoadScene("DisplayScore");
                }
                break;
        }
    }
}
