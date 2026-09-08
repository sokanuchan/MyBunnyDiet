using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    private struct ShopItem
    {
        public int cost;
        public string content;

        public ShopItem(int cost, string content)
        {
            this.cost = cost;
            this.content = content;
        }
    }

    private List<ShopItem> shopItems = new List<ShopItem>
    {
        new ShopItem(80, "Sortie musée"),
        new ShopItem(60, "Atelier peinture"),
        new ShopItem(30, "lecture d'une histoire"),
        new ShopItem(60, "sortie patin a glace"),
        new ShopItem(40, "seance d'escalade"),
        new ShopItem(50, "Activite manuelle au choix"),
        new ShopItem(70, "Pique-nique"),
        new ShopItem(60, "Ton meilleur ami te cuisine le repas de ton choix"),
        new ShopItem(15, "Regarder des lapins sur insta 15 minutes"),
    };
    private int shopItemCurrentIndex = 0;
    private int shopItemCurrentIndexModifier = 0;
    private GameObject highlight;

    public List<Text> shopItemContents;
    public List<Text> shopItemCosts;
    public List<GameObject> shopItemsUI;
    public GameObject UpButton;
    public GameObject DownButton;
    public Text bunnyStarsText;

    public static int bunnyStars = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // create the highlight
        highlight = Instantiate(shopItemsUI[0]);
        highlight.GetComponent<SpriteRenderer>().material.shader = Shader.Find("GUI/Text Shader");
        highlight.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
        highlight.transform.position = shopItemsUI[0].transform.position + new Vector3(0, 0, -1);

        // fill in the shop items info of the first 3 items
        UpdateShopItemsUI(true);
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
                shopItemCurrentIndex -= 1;
                UpdateShopItemsUI(true);
                break;
            case "Down":
                shopItemCurrentIndex += 1;
                UpdateShopItemsUI(false);
                break;
            case "Buy":
                BuyCurrentSelectedItem();
                break;
            case "Back":
                SceneManager.LoadScene("MiniGameMenu");
                break;
        }

        // update bunny stars
        bunnyStarsText.text = bunnyStars.ToString();
    }

    private void BuyCurrentSelectedItem()
    {
        // check if enough money
        ShopItem currentSelectedItem = shopItems[shopItemCurrentIndex];
        if (bunnyStars < currentSelectedItem.cost)
        {
            return;
        }

        // buy the item
        bunnyStars -= currentSelectedItem.cost;
        BoughtItemsManager.boughtItems.Add(currentSelectedItem.content);
        SaveManager.Save();
    }

    private void UpdateShopItemsUI(bool isMovingUp)
    {
        // remove up if at top of the list
        UpButton.SetActive(shopItemCurrentIndex != 0);

        // remove down button if at end of the list
        DownButton.SetActive(shopItemCurrentIndex != shopItems.Count - 1);

        // move highlight up or down
        if (isMovingUp && shopItemCurrentIndexModifier < 0)
        {
            shopItemCurrentIndexModifier += 1;
        }
        else if (!isMovingUp && shopItemCurrentIndexModifier > -2)
        {
            shopItemCurrentIndexModifier -= 1;
        }
        highlight.transform.position = shopItemsUI[-shopItemCurrentIndexModifier].transform.position + new Vector3(0, 0, -1);

        // update content and costs
        for (int shopItemIndex = 0; shopItemIndex < shopItemContents.Count; shopItemIndex++)
        {
            shopItemContents[shopItemIndex].text = shopItems[shopItemCurrentIndex + shopItemCurrentIndexModifier + shopItemIndex].content;
            shopItemCosts[shopItemIndex].text = shopItems[shopItemCurrentIndex + shopItemCurrentIndexModifier + shopItemIndex].cost.ToString();
        }
    }
}
