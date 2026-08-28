using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class CalendarManager : MonoBehaviour
{
    public Text displayedDate;
    public GameObject[] days;
    public GameObject[] daysImages;
    private List<Vector3> daysImagesPosTmp = new List<Vector3>();
    private List<Vector3> daysImagesScaleTmp = new List<Vector3>();
    public Text[] dayIndices;
    public GameObject dayOutline;

    private int currentYear;
    private int currentMonth;
    private CalendarMode calendarMode = CalendarMode.DayIndex;
    private Dictionary<CalendarMode, GameObject> CalendarModeButtons = new Dictionary<CalendarMode, GameObject>();

    private enum CalendarMode
    {
        DayIndex,
        Mood,
        Calories,
        Sport,
        Activity
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // init day image tmp lists
        foreach (GameObject daysImage in daysImages)
        {
            daysImagesPosTmp.Add(daysImage.transform.position);
            daysImagesScaleTmp.Add(daysImage.transform.localScale);
        }

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
        CalendarModeButtons[CalendarMode.Activity] = GameObject.Find("ActivityButton");
        CalendarModeButtons[CalendarMode.Calories] = GameObject.Find("CaloriesButton");
        CalendarModeButtons[CalendarMode.Mood] = GameObject.Find("MoodButton");
        CalendarModeButtons[CalendarMode.Sport] = GameObject.Find("SportButton");
    }

    private void DisplayRedOutlineOnCurrentDay()
    {
        // check if current day is displayed
        if (DateTime.Now.Month != currentMonth)
        {
            dayOutline.SetActive(false);
            return;
        }

        // find current day
        dayOutline.SetActive(true);
        GameObject currentDay = GameObject.Find("day_" + DateTime.Now.Day.ToString());
        dayOutline.transform.position = currentDay.transform.Find("DayImage").position + new Vector3(0, 0, -3);
    }

    private void UpdateDaysInMonth()
    {
        int nbDaysInMonth = DateTime.DaysInMonth(currentYear, currentMonth);

        // restore day image
        for (int dayImageIndex = 0; dayImageIndex < daysImages.Length; dayImageIndex++)
        {
            daysImages[dayImageIndex].transform.position = daysImagesPosTmp[dayImageIndex];
            daysImages[dayImageIndex].transform.localScale = daysImagesScaleTmp[dayImageIndex];
        }

        for (int dayIndex = 1; dayIndex <= 31; dayIndex++)
        {
            // check if day is present in month
            bool dayPresentInMonth = dayIndex <= nbDaysInMonth;

            // activate or deactivate day depending on month
            GameObject currentDay = days[dayIndex - 1];
            currentDay.SetActive(dayPresentInMonth);
            if (!dayPresentInMonth)
            {
                continue;
            }

            // check if day is inputable (must not be after currentInputDate)
            DateTime currentInputDate = DateUtils.GetCurrentInputDate();
            DateTime currentDate = new DateTime(currentYear, currentMonth, dayIndex);
            bool isInputable = currentDate <= currentInputDate;

            // gray out not inputable days
            SpriteRenderer dayImage = daysImages[dayIndex - 1].GetComponent<SpriteRenderer>();
            daysImages[dayIndex - 1].SetActive(!isInputable);
            daysImages[dayIndex - 1].transform.position = new Vector3(daysImages[dayIndex - 1].transform.position.x, daysImages[dayIndex - 1].transform.position.y, -1);
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
                daysImages[dayIndex - 1].transform.position = new Vector3(daysImages[dayIndex - 1].transform.position.x, daysImages[dayIndex - 1].transform.position.y, -3);
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
            if (calendarMode == CalendarMode.Calories)
            {
                daysImages[dayIndex - 1].SetActive(true);
                dayImage.sprite = Resources.Load<Sprite>("Images/UI/Square");

                if (DailyInput.playerInputs.ContainsKey(currentDateStr))
                {
                    ScoreManager.ScoreChanges scoreChanges = new ScoreManager.ScoreChanges();
                    ScoreManager.ChangeCaloriesScore(scoreChanges, DailyInput.playerInputs[currentDateStr].calories, currentDate);
                    dayIndices[dayIndex - 1].text = scoreChanges.totalChanges.ToString();
                    dayImage.color = GetScoreColor(scoreChanges.totalChanges);
                }
                else
                {
                    dayIndices[dayIndex - 1].text = "0";
                    dayImage.color = new Color(0, 0, 0, 0.8f);
                }
                dayIndices[dayIndex - 1].fontSize = 100;
            }

            // Activity
            if (isInputable
                && calendarMode == CalendarMode.Activity)
            {
                daysImages[dayIndex - 1].SetActive(true);
                daysImages[dayIndex - 1].transform.localScale = Vector3.one * 0.5f;
                daysImages[dayIndex - 1].transform.position = new Vector3(daysImages[dayIndex - 1].transform.position.x, daysImages[dayIndex - 1].transform.position.y, -3);
                if (DailyInput.playerInputs.ContainsKey(currentDateStr))
                {
                    int currentActivity = DailyInput.playerInputs[currentDateStr].activity;
                    dayImage.color = GetActivityColor(currentActivity);
                    Debug.Log(dayImage.color.ToString() + " " + currentDateStr);
                    dayImage.sprite = Resources.Load<Sprite>("Images/UI/Square");
                    dayImage.transform.localScale = new Vector3(0.7f, currentActivity / 100f * 0.7f, 0);
                    dayImage.transform.position += new Vector3(0, -0.325f + currentActivity / 325f, 0);
                }
                else
                {
                    dayImage.sprite = null;
                }
            }

            // Sport
            if (calendarMode == CalendarMode.Sport)
            {
                daysImages[dayIndex - 1].SetActive(true);
                if (DailyInput.playerInputs.ContainsKey(currentDateStr))
                {
                    dayIndices[dayIndex - 1].text = (DailyInput.playerInputs[currentDateStr].walk 
                                                    + DailyInput.playerInputs[currentDateStr].muscu 
                                                    + DailyInput.playerInputs[currentDateStr].cardio).ToString();

                    ScoreManager.ScoreChanges scoreChanges = new ScoreManager.ScoreChanges();
                    ScoreManager.ChangeSportScore(scoreChanges, ScoreManager.walkRatio, "", DailyInput.playerInputs[currentDateStr].walk);
                    ScoreManager.ChangeSportScore(scoreChanges, ScoreManager.muscuRatio, "", DailyInput.playerInputs[currentDateStr].muscu);
                    ScoreManager.ChangeSportScore(scoreChanges, ScoreManager.cardioRatio, "", DailyInput.playerInputs[currentDateStr].cardio);
                    dayIndices[dayIndex - 1].text = scoreChanges.totalChanges.ToString();
                    dayImage.color = GetScoreColor(scoreChanges.totalChanges);
                }
                else
                {
                    dayIndices[dayIndex - 1].text = "0";
                    dayImage.color = new Color(0, 0, 0, 0.8f);
                }
                dayIndices[dayIndex - 1].fontSize = 100;
            }
        }

        // display red outline on current day if it is in current displayed month
        DisplayRedOutlineOnCurrentDay();
    }

    private Color GetScoreColor(int score)
    {
        // positive score
        if (score >= 0)
        {
            return new Color(1 - 0.003f * score, 1, 1 - 0.003f * score);
        }

        // negative score
        return new Color(1,1 + 0.003f * score, 1 + 0.003f * score);
    }

    private Color GetActivityColor(int activity)
    {
        // The bluer the smaller the activity is, the greener the higher
        return new Color(0, activity / 100f, (100 - activity) / 100f);
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
            case "ActivityButton":
                ChangeCalendarMode(CalendarMode.Activity);
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
        // darken old button
        if (calendarMode != CalendarMode.DayIndex)
        {
            CalendarModeButtons[calendarMode].GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
        }

        // go to day index mode button is unchecked
        if (newCalendarMode == calendarMode)
        {
            calendarMode = CalendarMode.DayIndex;
            return;
        }

        // highlight new mode
        calendarMode = newCalendarMode;
        CalendarModeButtons[calendarMode].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
    }
}
