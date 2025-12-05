using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BunniesMenu : MonoBehaviour
{
    public List<GameObject> bunnies;
    public GameObject left;
    public GameObject right;
    public GameObject bunnyView;
    public BoxCollider2D homeButton;
    public SpriteRenderer pixelButton;

    public static int pageIndex = 0;

    private bool isPixelButtonSelected = false;
    private int currentBunny = 0;

    public static List<int> unlockedBunnies = new List<int>() { };

    // constants
    private int nbBunnies = 30;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bunnyView.SetActive(false);
        UpdateBunnies();
    }

    private void UpdateBunnies()
    {
        // update left and right
        left.SetActive(pageIndex > 0);
        right.SetActive(pageIndex + 1 < nbBunnies / bunnies.Count);

        // update each bunny
        for (int i = 0; i < bunnies.Count; ++i)
        {
            int bunnyNb = pageIndex * bunnies.Count + i + 1;

            // already done bunnies
            SpriteRenderer bunnySR = bunnies[i].transform.Find("bunnyPreview").GetComponent<SpriteRenderer>();
            if (unlockedBunnies.Contains(bunnyNb))
            {
                bunnySR.sprite = Resources.Load<Sprite>("Images/Bunnies/Bunny-" + bunnyNb.ToString() + "/CompletePixelArt");
            }
            else
            {
                bunnySR.sprite = null;
            }

            // locked / unlocked bunnies
            if (bunnyNb <= (ScoreManager.nbBunnyParts / 7) + 1)
            {
                bunnies[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/UI/Bunnies/QuestionMarkWhite");
            }
            else
            {
                bunnies[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/UI/Bunnies/QuestionMark");
            }

            // update bunny indices
            bunnies[i].transform.Find("BunnyIndex").GetComponent<Text>().text = bunnyNb.ToString();
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
            case "Left":
                pageIndex--;
                UpdateBunnies();
                break;
            case "Right":
                pageIndex++;
                UpdateBunnies();
                break;
            case "Back":
                BunnyViewDisplay(false);
                break;
            case "Pixel":
                HandlePixelButton();
                break;

        }

        // handle bunny
        if (hitButton.StartsWith("Bunny"))
        {
            int bunnyNb = int.Parse(GameObject.Find(hitButton).transform.Find("BunnyIndex").GetComponent<Text>().text);
            if (unlockedBunnies.Contains(bunnyNb))
            {
                ShowBunny(bunnyNb);
            }
            else if (bunnyNb <= (ScoreManager.nbBunnyParts / 7) + 1)
            {
                SquaresImageLoader.nbBunnyParts = Mathf.Min(ScoreManager.nbBunnyParts - (bunnyNb - 1) * 8, 8);
                SquaresImageLoader.bunnyIndex = bunnyNb;
                SceneManager.LoadScene("SlidingPuzzle");
            }
        }
    }

    private void ShowBunny(int bunnyNb)
    {
        BunnyViewDisplay(true);
        bunnyView.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/Bunnies/Bunny-" + bunnyNb.ToString() + "/Complete");
        currentBunny = bunnyNb;
    }

    private void BunnyViewDisplay(bool show)
    {
        bunnyView.SetActive(show);

        homeButton.enabled = !show;
        left.GetComponent<CircleCollider2D>().enabled = !show;
        right.GetComponent<CircleCollider2D>().enabled = !show;
        foreach (GameObject bunny in bunnies)
        {
            bunny.GetComponent<BoxCollider2D>().enabled = !show;
        }

        isPixelButtonSelected = !show;
        pixelButton.sprite = Resources.Load<Sprite>("Images/UI/Bunnies/PixelNotSelected");
    }

    private void HandlePixelButton()
    {
        isPixelButtonSelected = !isPixelButtonSelected;

        if (!isPixelButtonSelected)
        {
            bunnyView.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/Bunnies/Bunny-" + currentBunny.ToString() + "/Complete");
            pixelButton.sprite = Resources.Load<Sprite>("Images/UI/Bunnies/PixelNotSelected");
        }
        else
        {
            bunnyView.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/Bunnies/Bunny-" + currentBunny.ToString() + "/CompletePixelArt");
            pixelButton.sprite = Resources.Load<Sprite>("Images/UI/Bunnies/PixelSelected");
        }
    }
}
