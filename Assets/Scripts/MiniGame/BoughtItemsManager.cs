using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BoughtItemsManager : MonoBehaviour
{
    public static List<string> boughtItems = new List<string> ();
    private int boughtItemCurrentIndex = 0;
    private int highlightPos = 0;
    private GameObject highlight;

    public List<Text> boughtItemContents;
    public List<GameObject> boughtItemsUI;
    public GameObject UpButton;
    public GameObject DownButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // create the highlight
        highlight = Instantiate(boughtItemsUI[0]);
        highlight.GetComponent<SpriteRenderer>().material.shader = Shader.Find("GUI/Text Shader");
        highlight.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
        highlight.transform.position = boughtItemsUI[0].transform.position + new Vector3(0, 0, -1);

        // fill in the shop items info of the first 3 items
        UpdateBoughtItemsUI("");
    }

    // Update is called once per frame
    void Update()
    {
        // get hit button
        string hitButton = MenuManager.GetHitButton();

        // handle hit button
        switch (hitButton)
        {
            case "Up":
                UpdateBoughtItemsUI("Up");
                break;
            case "Down":
                UpdateBoughtItemsUI("Down");
                break;
            case "Use":
                UseCurrentSelectedItem();
                break;
            case "Back":
                SceneManager.LoadScene("MiniGameMenu");
                break;
        }
    }

    private void UseCurrentSelectedItem()
    {
        // Check if item is in the list
        if (boughtItemCurrentIndex >= boughtItems.Count)
        {
            return;
        }

        // remove the item off the list
        boughtItems.Remove(boughtItems[boughtItemCurrentIndex]);
        SaveManager.Save();

        // update UI
        UpdateBoughtItemsUI("");
    }

    private void UpdateBoughtItemsUI(string direction)
    {
        // Move up
        if (direction == "Up")
        {
            boughtItemCurrentIndex -= 1;
            if (highlightPos < 0)
            {
                highlightPos += 1;
            }
        }

        // Move down
        if (direction == "Down")
        {
            boughtItemCurrentIndex += 1;
            if (highlightPos > -2)
            {
                highlightPos -= 1;
            }
        }
        highlight.transform.position = boughtItemsUI[-highlightPos].transform.position + new Vector3(0, 0, -1);

        // remove up if at top of the list
        UpButton.SetActive(boughtItemCurrentIndex != 0);

        // remove down button if at end of the list
        DownButton.SetActive(boughtItemCurrentIndex < boughtItems.Count - 1);

        // update content
        for (int shopItemIndex = 0; shopItemIndex < boughtItemContents.Count; shopItemIndex++)
        {
            if (boughtItemCurrentIndex + highlightPos + shopItemIndex < boughtItems.Count)
            {
                boughtItemContents[shopItemIndex].text = boughtItems[boughtItemCurrentIndex + highlightPos + shopItemIndex];
            }
            else
            {
                boughtItemContents[shopItemIndex].text = "";
            }
        }
    }
}
