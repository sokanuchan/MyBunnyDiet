using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplayer : MonoBehaviour
{
    public Text bonus;
    public Text malus;
    public Text total;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DisplayScoreChanges();
    }

    private void DisplayScoreChanges()
    {
        // display bonus
        bonus.text = "";
        foreach (string positiveChange in ScoreManager.currentScoreChanges.positiveChanges)
        {
            bonus.text += "• +" + positiveChange + "\n";
        }

        // display malus
        malus.text = "";
        foreach (string negativeChange in ScoreManager.currentScoreChanges.negativeChanges)
        {
            malus.text += "• " + negativeChange + "\n";
        }

        // display total
        total.text = ScoreManager.currentScoreChanges.totalChanges.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
