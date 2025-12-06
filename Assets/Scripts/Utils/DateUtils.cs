using System;
using System.Collections.Generic;
using System.Linq;

public class DateUtils
{
    public static string dailyInputDateFormat = "yyyy-MM-dd";

    public static string GetMonthName(int monthIndex)
    {
        List<string> months = new List<string>()
        {
            "Janvier",
            "Fevrier",
            "Mars",
            "Avril",
            "Mais",
            "Juin",
            "Juillet",
            "Aout",
            "Septembre",
            "Octobre",
            "Novembre",
            "Decembre"
        }; 
        
        return months[monthIndex - 1];
    }

    public static DateTime GetCurrentInputDate()
    {
        if (DailyInput.playerInputs.Count == 0)
        {
            return DateTime.MaxValue;
        }

        DateTime lastInputDate = DateTime.ParseExact(DailyInput.playerInputs.Last().Key, dailyInputDateFormat, null);
        return lastInputDate.AddDays(1);
    }
}
