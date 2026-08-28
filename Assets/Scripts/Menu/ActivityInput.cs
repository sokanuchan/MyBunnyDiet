using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ActivityInput : MonoBehaviour
{
    public Slider activitySlider;

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
            case "Validate":
                DailyInput.currentDailyInput.activity = (int)(activitySlider.value * 100);
                Debug.Log(DailyInput.currentDailyInput.activity);
                break;
            default:
                return;
        }

        SceneManager.LoadScene("DailyInputs");
    }
}
