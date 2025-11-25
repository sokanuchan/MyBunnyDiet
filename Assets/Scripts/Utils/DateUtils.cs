using System.Collections.Generic;

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
}
