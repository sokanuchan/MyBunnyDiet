using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class CalendarManager : MonoBehaviour
{
    public Text displayedDate;
    public GameObject[] days;
    public GameObject[] daysImages;
    public Text[] dayIndices;

    private int currentYear;
    private int currentMonth;
    private CalendarMode calendarMode = CalendarMode.DayIndex;
    private Dictionary<CalendarMode, GameObject> CalendarModeButtons = new Dictionary<CalendarMode, GameObject>();

    private enum CalendarMode
    {
        DayIndex,
        Mood,
        Calories,
        Sport
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentYear = DateTime.Now.Year;
        currentMonth = DateTime.Now.Month;
        DisplayMonth();
        InitCalendarModeButtons();
    }

    private void DisplayMonth()
    {
        // display month name + year
        displayedDate.text = DateUtils.GetMonthName(currentMonth) + " " + currentYear.ToString();

        // display the right amount of days in current month
        UpdateDaysInMonth();
    }

    void InitCalendarModeButtons()
    {
        CalendarModeButtons[CalendarMode.DayIndex] = GameObject.Find("DayIndexButton");
        CalendarModeButtons[CalendarMode.Calories] = GameObject.Find("CaloriesButton");
        CalendarModeButtons[CalendarMode.Mood] = GameObject.Find("MoodButton");
        CalendarModeButtons[CalendarMode.Sport] = GameObject.Find("SportButton");
    }

    private void UpdateDaysInMonth()
    {
        int nbDaysInMonth = DateTime.DaysInMonth(currentYear, currentMonth);

        for (int dayIndex = 1; dayIndex <= 31; dayIndex++)
        {
            // check if day is present in month
            bool dayPresentInMonth = dayIndex <= nbDaysInMonth;

            // activate or deactivate day depending on month
            GameObject currentDay = days[dayIndex - 1];
            currentDay.SetActive(dayPresentInMonth);
            if (!dayPresentInMonth)
            {
                return;
            }

            // check if day is inputable (must not be after currentInputDate)
            DateTime currentInputDate = DateUtils.GetCurrentInputDate();
            DateTime currentDate = new DateTime(currentYear, currentMonth, dayIndex);
            bool isInputable = currentDate <= currentInputDate;

            // gray out not inputable days
            SpriteRenderer dayImage = daysImages[dayIndex - 1].GetComponent<SpriteRenderer>();
            daysImages[dayIndex - 1].SetActive(!isInputable);
            dayImage.sprite = Resources.Load<Sprite>("Images/UI/Square");
            dayImage.color = new Color(0, 0, 0, 0.8f);
            daysImages[dayIndex - 1].transform.localScale = Vector3.one * 0.75f;
            currentDay.GetComponent<BoxCollider2D>().enabled = isInputable;

            // Mood
            string currentDateStr = currentDate.ToString(DateUtils.dailyInputDateFormat);
            if (isInputable
                && calendarMode == CalendarMode.Mood)
            {
                daysImages[dayIndex - 1].SetActive(true);
                dayImage.color = new Color(1, 1, 1, 0.9f);
                daysImages[dayIndex - 1].transform.localScale = Vector3.one * 0.5f;
                if (DailyInput.playerInputs.ContainsKey(currentDateStr))
                {
                    dayImage.sprite = Resources.Load<Sprite>("Images/UI/Mood/" + DailyInput.playerInputs[currentDateStr].mood.ToString());
                }
                else
                {
                    dayImage.sprite = null;
                }
            }

            // Day Index
            if (calendarMode == CalendarMode.DayIndex || calendarMode == CalendarMode.Mood)
            {
                dayIndices[dayIndex - 1].text = dayIndex.ToString();
                dayIndices[dayIndex - 1].fontSize = 200;
            }

            // Calories
            if (isInputable && calendarMode == CalendarMode.Calories)
            {
                if (DailyInput.playerInputs.ContainsKey(currentDateStr))
                {
                    dayIndices[dayIndex - 1].text = DailyInput.playerInputs[currentDateStr].calories.ToString();
                }
                else
                {
                    dayIndices[dayIndex - 1].text = "0";
                }
                dayIndices[dayIndex - 1].fontSize = 100;
            }

            // Sport
            if (isInputable && calendarMode == CalendarMode.Sport)
            {
                if (DailyInput.playerInputs.ContainsKey(currentDateStr))
                {
                    dayIndices[dayIndex - 1].text = (DailyInput.playerInputs[currentDateStr].walk 
                                                    + DailyInput.playerInputs[currentDateStr].muscu 
                                                    + DailyInput.playerInputs[currentDateStr].cardio).ToString();
                }
                else
                {
                    dayIndices[dayIndex - 1].text = "0";
                }
                dayIndices[dayIndex - 1].fontSize = 100;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // get hit button
        string hitButton = MenuManager.GetHitButton();

        // handle days
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
            return;
        }

        // handle buttons
        switch (hitButton)
        {
            case "PreviousMonth":
                ChangeCurentMonth(-1);
                break;
            case "NextMonth":
                ChangeCurentMonth(+1);
                break;
            case "CaloriesButton":
                ChangeCalendarMode(CalendarMode.Calories);
                break;
            case "MoodButton":
                ChangeCalendarMode(CalendarMode.Mood);
                break;
            case "DayIndexButton":
                ChangeCalendarMode(CalendarMode.DayIndex);
                break;
            case "SportButton":
                ChangeCalendarMode(CalendarMode.Sport);
                break;
            default:
                return;
        }
        DisplayMonth();
    }

    void ChangeCurentMonth(int monthModifier)
    {
        DateTime currentMonthDate = new DateTime(currentYear, currentMonth, 1);
        DateTime monthAfterModification = currentMonthDate.AddMonths(monthModifier);
        currentMonth = monthAfterModification.Month;
        currentYear = monthAfterModification.Year;
    }

    void ChangeCalendarMode(CalendarMode newCalendarMode)
    {
        CalendarModeButtons[calendarMode].GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f);
        calendarMode = newCalendarMode;
        CalendarModeButtons[calendarMode].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
    }
}
