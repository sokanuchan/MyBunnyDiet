using System;
using UnityEngine;
using UnityEngine.UI;

public class CalendarDisplayer : MonoBehaviour
{
    public Text displayedDate;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DisplayMonth(DateTime.Now.Year, DateTime.Now.Month);
    }

    private void DisplayMonth(int year, int month)
    {
        displayedDate.text = DateUtils.GetMonthName(month) + " " + year.ToString();
        Debug.Log(displayedDate.text);

        UpdateDaysInMonth(year, month);
    }

    private void UpdateDaysInMonth(int year, int month)
    {
        int nbDaysInMonth = DateTime.DaysInMonth(year, month);

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
        
    }
}
