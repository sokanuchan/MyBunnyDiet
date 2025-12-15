using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SaveManager.Load();
    }

    // Update is called once per frame
    void Update()
    {
        // get hit button
        string hitButton = MenuManager.GetHitButton();

        // handle hit button
        switch (hitButton)
        {
            case "DailyInputs":
                SceneManager.LoadScene("Calendar");
                break;
            case "Bunnies":
                SceneManager.LoadScene("Bunnies");
                break;
            case "Advices":
                SceneManager.LoadScene("Advices");
                break;
        }
    }
}
