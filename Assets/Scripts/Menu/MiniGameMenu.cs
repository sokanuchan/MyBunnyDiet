using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniGameMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FindFirstObjectByType<AudioManager>().StopAll();
        FindFirstObjectByType<AudioManager>().Play("MiniGame - Menu");
    }

    // Update is called once per frame
    void Update()
    {
        // get hit button
        string hitButton = MenuManager.GetHitButton();

        // handle hit button
        switch (hitButton)
        {
            case "Play":
                LevelManager.Reset();
                SceneManager.LoadScene("LevelTransition");
                break;
            case "Shop":
                SceneManager.LoadScene("Shop");
                break;
            case "BoughtItems":
                SceneManager.LoadScene("BoughtItems");
                break;
        }
    }
}
