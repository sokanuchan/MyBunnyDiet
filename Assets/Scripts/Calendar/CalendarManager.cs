using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CalendarManager : MonoBehaviour
{
    public Text displayedDate;

    private int currentYear;
    private int currentMonth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentYear = DateTime.Now.Year;
        currentMonth = DateTime.Now.Month;
        DisplayMonth();
    }

    private void DisplayMonth()
    {
        // display month name + year
        displayedDate.text = DateUtils.GetMonthName(currentMonth) + " " + currentYear.ToString();

        // display the right amount of days in current month
        UpdateDaysInMonth();
    }

    private void UpdateDaysInMonth()
    {
        int nbDaysInMonth = DateTime.DaysInMonth(currentYear, currentMonth);

        for (int dayIndex = 1; dayIndex <= 31; dayIndex++)
        {
            // check if day is present in month
            bool dayPresentInMonth = dayIndex <= nbDaysInMonth;

            // activate or deactivate day
            GameObject.Find("day_" + dayIndex.ToString()).SetActive(dayPresentInMonth);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // get hit button
        string hitButton = MenuManager.GetHitButton();

        // handle hit button
        int dayIndex;
        if (hitButton.Contains("day_") 
            && int.TryParse(hitButton.Substring("day_".Length), out dayIndex))
        {
            DailyInput.currentDate = new DateTime(currentYear, currentMonth, dayIndex).ToString(DateUtils.dailyInputDateFormat);
            if (DailyInput.playerInputs.ContainsKey(DailyInput.currentDate))
            {
                DailyInput.currentDailyInput = DailyInput.playerInputs[DailyInput.currentDate];
            }
            else
            {
                DailyInput.currentDailyInput = new DailyInput();
            }
            SceneManager.LoadScene("DailyInputs");
        }
    }
}
