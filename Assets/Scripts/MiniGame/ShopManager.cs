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
        new ShopItem(15, "Etre une princesse pendant 5 minutes"),
        new ShopItem(30, "Tester un nouveau plat de ton choix, fait maison"),
        new ShopItem(40, "On invente tous les deux une choregraphie sur une musique de ton choix, on se la montre a la fin de la semaine"),
        new ShopItem(80, "Sortie musée"),
        new ShopItem(15, "Atelier peinture aquarelle"),
        new ShopItem(20, "Soirée jeux sur ordis à distance")
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
