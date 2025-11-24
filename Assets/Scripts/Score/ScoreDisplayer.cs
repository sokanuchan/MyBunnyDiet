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
        // Get score changes
        ScoreManager.ScoreChanges scoreChanges = ScoreManager.GetScoreChanges(DailyInput.currentDailyInput);

        // display bonus
        bonus.text = "";
        foreach (string positiveChange in scoreChanges.positiveChanges)
        {
            bonus.text += "• +" + positiveChange + "\n";
        }

        // display malus
        malus.text = "";
        foreach (string negativeChange in scoreChanges.negativeChanges)
        {
            malus.text += "• " + negativeChange + "\n";
        }

        // display total
        total.text = scoreChanges.totalChanges.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
