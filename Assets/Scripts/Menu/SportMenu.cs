using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SportMenu : MonoBehaviour
{
    public Text currentMuscu;
    public Text currentWalk;
    public Text currentCardio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // update current sports with current input values
        currentMuscu.text = "Current: " + DailyInput.currentDailyInput.muscu;
        currentWalk.text = "Current: " + DailyInput.currentDailyInput.walk;
        currentCardio.text = "Current: " + DailyInput.currentDailyInput.cardio;
    }

    // Update is called once per frame
    void Update()
    {
        // get hit button
        string hitButton = MenuManager.GetHitButton();

        // handle hit button
        switch (hitButton)
        {
            case "Muscu":
                KeyboardManager.GetInput("Combien de series ?", hitButton);
                break;
            case "Walk":
                KeyboardManager.GetInput("Combien de metres marches ?", hitButton);
                break;
            case "Cardio":
                KeyboardManager.GetInput("Combien de minutes ?", hitButton);
                break;
            case "Back":
                SceneManager.LoadScene("DailyInputs");
                break;
        }
    }
}
