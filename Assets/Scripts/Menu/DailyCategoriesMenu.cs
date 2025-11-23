using UnityEngine;
using UnityEngine.SceneManagement;

public class DailyCategoriesMenu : MonoBehaviour
{
    private static bool caloriesButtonClicked = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!caloriesButtonClicked)
        {
            GameObject.Find("Validate").GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // get hit button
        string hitButton = MenuManager.GetHitButton();

        // handle hit button
        switch (hitButton)
        {
            case "Calories":
                caloriesButtonClicked = true;
                KeyboardManager.GetInput("Combien de calories aujourd'hui ?", hitButton);
                break;
            case "Sport":
                SceneManager.LoadScene("SportInputs");
                break;
            case "Validate":
                if (caloriesButtonClicked)
                {
                    SceneManager.LoadScene("DisplayScore");
                }
                break;
        }
    }
}
