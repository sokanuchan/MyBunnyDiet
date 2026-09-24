using UnityEngine;
using UnityEngine.InputSystem;

public class InitMeasures : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // init measurements only if they are missing
        if (ScoreManager.age == 0)
        {
            KeyboardManager.GetInput("Quel est ton age ?\n(en annees)", "Age");
        }
        else if (ScoreManager.height == 0)
        {
            KeyboardManager.GetInput("Quel est ta taille ?\n(en cm)", "Height");
        }
        else if (ScoreManager.startingWeight == 0)
        {
            KeyboardManager.GetInput("Quel est ton poids ?\n(en Kg)", "StartingWeight");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
