using UnityEngine;
using UnityEngine.SceneManagement;

public class SportMenu : MonoBehaviour
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
