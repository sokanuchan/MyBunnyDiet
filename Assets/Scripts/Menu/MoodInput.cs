using UnityEngine;
using UnityEngine.SceneManagement;

public class MoodInput : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // get hit button
        string hitButton = MenuManager.GetHitButton();

        // handle hit button
        switch (hitButton)
        {
            case "Back":
                break;
            case "MoodVeryHappy":
                DailyInput.currentDailyInput.mood = DailyInput.Mood.VeryHappy;
                break;
            case "MoodHappy":
                DailyInput.currentDailyInput.mood = DailyInput.Mood.Happy;
                break;
            case "MoodNeutral":
                DailyInput.currentDailyInput.mood = DailyInput.Mood.Neutral;
                break;
            case "MoodSad":
                DailyInput.currentDailyInput.mood = DailyInput.Mood.Sad;
                break;
            case "MoodVerySad":
                DailyInput.currentDailyInput.mood = DailyInput.Mood.VerySad;
                break;
            default:
                return;
        }

        SceneManager.LoadScene("DailyInputs");
    }
}
