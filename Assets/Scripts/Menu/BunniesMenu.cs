using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BunniesMenu : MonoBehaviour
{
    public List<GameObject> bunnies;
    public GameObject left;
    public GameObject right;

    private int pageIndex = 0;

    private static List<int> unlockedBunnies = new List<int>() { 1 };

    // constants
    private int nbBunnies = 30;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
            Debug.Log(bunnyNb);

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
            print(ScoreManager.nbBunnyParts / 7);
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
        }

        // handle bunny
        if (hitButton.StartsWith("Bunny"))
        {
            int bunnyNb = int.Parse(GameObject.Find(hitButton).transform.Find("BunnyIndex").GetComponent<Text>().text);
            ShowBunny(bunnyNb);
        }
    }

    private void ShowBunny(int bunnyNb)
    {

    }
}
