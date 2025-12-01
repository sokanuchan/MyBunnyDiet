using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static ScoreManager;

public class ScoreDisplayer : MonoBehaviour
{
    public Text bonus;
    public Text malus;
    public Text total;

    public GameObject scorePorgressionPopup;
    public Slider scoreSlider;
    public Animator hopingBunny;
    public Text scoreProgressLowerBound;
    public Text scoreProgressUpperBound;
    public Text currentScore;
    public Transform bunnyStartPos;
    public Transform bunnyEndPos;
    public GameObject scoreDisplay;

    private int scoreProgressStart;
    private int scoreProgressEnd;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scorePorgressionPopup.SetActive(false);

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
        // get hit button
        string hitButton = MenuManager.GetHitButton();

        // handle hit button
        switch (hitButton)
        {
            case "ShowProgression":
                StartCoroutine("ShowScoreProgressionPopup");
                break;
        }
    }

    private IEnumerator ShowScoreProgressionPopup()
    {
        // activate popup
        scorePorgressionPopup.SetActive(true);
        scoreDisplay.SetActive(false);

        // get bunnyParts before score update
        int nbBunnyParts = ScoreManager.nbBunnyParts;

        // get how much total score to add
        int startScore = ScoreManager.totalScore;
        ScoreManager.UpdateScore();
        int endScore = ScoreManager.totalScore;
        int scoreLeftToAdd = endScore - startScore;

        // add score in chunks of 1000, because the bars are 1000 points long
        while (scoreLeftToAdd > 0)
        {
            // get start and end of current progress
            scoreProgressStart = startScore - nbBunnyParts * 1000;
            scoreProgressEnd = Mathf.Min(scoreProgressStart + scoreLeftToAdd, 1000);

            // update bounds
            scoreProgressLowerBound.text = (nbBunnyParts * 1000).ToString();
            scoreProgressUpperBound.text = ((nbBunnyParts + 1) * 1000).ToString();
            nbBunnyParts += 1;

            // update one chunk
            yield return ScoreProgress();
            int scoreDiff = scoreProgressEnd - scoreProgressStart;
            scoreLeftToAdd -= scoreDiff;
            startScore += scoreDiff;

            // wait 1s before next chunk
            yield return new WaitForSeconds(1);
        }

        if (scoreLeftToAdd < 0)
        {
            // bunny u-turn
            hopingBunny.GetComponent<SpriteRenderer>().flipX = true;

            // get start and end of current progress
            scoreProgressStart = startScore % 1000;
            scoreProgressEnd = scoreProgressStart + scoreLeftToAdd;

            // update bounds
            scoreProgressLowerBound.text = (nbBunnyParts * 1000).ToString();
            scoreProgressUpperBound.text = ((nbBunnyParts + 1) * 1000).ToString();

            // update one chunk
            yield return ScoreProgress();
            int scoreDiff = scoreProgressEnd - scoreProgressStart;
            scoreLeftToAdd -= scoreDiff;
            startScore += scoreDiff;

            // bunny u-turn
            hopingBunny.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    private IEnumerator ScoreProgress()
    {
        // start bunny hops
        hopingBunny.SetBool("isHoping", true);

        // init score slider value
        scoreSlider.value = scoreProgressStart;

        // change score slider value within 2 seconds
        float scoreToAdd = scoreProgressEnd - scoreProgressStart;
        for (int frameIndex = 1; frameIndex <= 100; frameIndex++)
        {
            // update slider value
            scoreSlider.value = scoreProgressStart + scoreToAdd / 100 * frameIndex;

            // update current score
            currentScore.text = ((int)(scoreProgressStart + scoreToAdd / 100 * frameIndex)).ToString();

            // update bunny pos
            hopingBunny.transform.position = GetBunnyPos(scoreSlider.value);

            // wait for 2/100 s
            yield return new WaitForSeconds(2f / 100);
        }

        // make sure score slider value is exactly end value
        scoreSlider.value = scoreProgressEnd;

        // stop bunny hops
        hopingBunny.SetBool("isHoping", false);
    }

    private Vector3 GetBunnyPos(float scoreSliderValue)
    {
        return new Vector3(
            (bunnyEndPos.position.x - bunnyStartPos.position.x) * scoreSlider.value / 1000 + bunnyStartPos.position.x, 
            hopingBunny.transform.position.y, 
            hopingBunny.transform.position.z
            );
    }
}
