using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KeyboardManager : MonoBehaviour
{
    public Text output;
    public Text question;
   
    public static string inputType;
    public static string questionToAsk;
    public static string getBackScene = "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        question.text = questionToAsk;
    }

    // Update is called once per frame
    void Update()
    {
        string hitButton = MenuManager.GetHitButton();

        switch (hitButton)
        {
            // remove last char
            case "delete":
                if (output.text.Length > 0)
                {
                    output.text = output.text.Remove(output.text.Length - 1);
                }
                break;

            // validate player input
            case "enter":
                ValidatePlayerInput(int.Parse(output.text));
                break;

            // add pressed key to the output
            default:
                output.text += hitButton;
                break;
        }
    }

    private void ValidatePlayerInput(int playerInput)
    {
        // Change score depending on input value
        switch (inputType)
        {
            case "Calories":
                ScoreManager.ChangeCalories(playerInput);
                break;

            case "Muscu":
            case "Walk":
            case "Cardio":
                ScoreManager.ChangeSport(inputType, playerInput);
                break;
        }

        // get back to previous scene
        SceneManager.LoadScene(getBackScene);
    }

    public static void GetInput(string questionToAsk, string inputType)
    {
        KeyboardManager.questionToAsk = questionToAsk;
        KeyboardManager.inputType = inputType;
        getBackScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("KeyboardInput");
    }
}
