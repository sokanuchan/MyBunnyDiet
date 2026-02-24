using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BunnyManager : MonoBehaviour
{
    private float rightBoundary = 2.4f;
    private float leftBoundary = -2.4f;
    private bool isEating = false;
    private bool gameFinished = false;
    private Dictionary<string, Text> eatenFoodTexts = new Dictionary<string, Text>();
    private Dictionary<string, GameObject> eatenFoodUIs = new Dictionary<string, GameObject>();
    private Dictionary<string, int> eatenFood = new Dictionary<string, int>()
    {
        { "Carbs", 0 },
        { "Burger", 0 },
        { "Meat", 0 },
        { "Vegetable", 0 },
        { "Fat", 0 },
    };

    private static Dictionary<string, int> foodToEat = new Dictionary<string, int>();

    public GameObject carbsEatenFoodUI;
    public GameObject burgerEatenFoodUI;
    public GameObject meatEatenFoodUI;
    public GameObject vegetableEatenFoodUI;
    public GameObject fatEatenFoodUI;
    public GameObject victoryAnimation;
    public GameObject defeatBackground;
    public GameObject defeatAnimation;
    public GameObject bunnyStarEarningBackground;
    public Text bunnyStarEarningText;
    public Text loloText;

    public static float moveSpeed = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // init eatenFoodTexts dict
        eatenFoodTexts.Add("Carbs", carbsEatenFoodUI.transform.Find("EatenFoodText").GetComponent<Text>());
        eatenFoodTexts.Add("Burger", burgerEatenFoodUI.transform.Find("EatenFoodText").GetComponent<Text>());
        eatenFoodTexts.Add("Meat", meatEatenFoodUI.transform.Find("EatenFoodText").GetComponent<Text>());
        eatenFoodTexts.Add("Vegetable", vegetableEatenFoodUI.transform.Find("EatenFoodText").GetComponent<Text>());
        eatenFoodTexts.Add("Fat", fatEatenFoodUI.transform.Find("EatenFoodText").GetComponent<Text>());

        // init eatenFoodUIs dict
        eatenFoodUIs.Add("Carbs", carbsEatenFoodUI);
        eatenFoodUIs.Add("Burger", burgerEatenFoodUI);
        eatenFoodUIs.Add("Meat", meatEatenFoodUI);
        eatenFoodUIs.Add("Vegetable", vegetableEatenFoodUI);
        eatenFoodUIs.Add("Fat", fatEatenFoodUI);

        // only activate food ui for food to eat
        foreach (string foodName in foodToEat.Keys)
        {
            eatenFoodUIs[foodName].SetActive(true);
        }

        // start fight music
        FindFirstObjectByType<AudioManager>().StopAll();
        FindFirstObjectByType<AudioManager>().Play("MiniGame - Fight");
    }

    // Update is called once per frame
    void Update()
    {
        if (gameFinished)
        {
            return;
        }

        Move();
    }

    private void Move()
    {
        // no touch
        if (Input.touches.Length != 1)
        {
            if (!isEating)
            {
                GetComponent<Animator>().SetBool("isHoping", false);
                GetComponent<Rigidbody2D>().linearVelocityX = 0;
            }
            return;
        }

        // find where the player is currently touching the screen
        Touch touch = Input.touches[0];
        Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

        // Move right
        if (touchPos.x > 0 && transform.position.x < rightBoundary)
        {
            GetComponent<Animator>().SetBool("isHoping", true);
            GetComponent<Rigidbody2D>().linearVelocityX = moveSpeed;
            GetComponent<SpriteRenderer>().flipX = false;
            return;
        }

        // Move left
        if (touchPos.x < 0 && transform.position.x > leftBoundary)
        {
            GetComponent<Animator>().SetBool("isHoping", true);
            GetComponent<Rigidbody2D>().linearVelocityX = -moveSpeed;
            GetComponent<SpriteRenderer>().flipX = true;
            return;
        }

        GetComponent<Animator>().SetBool("isHoping", false);
        GetComponent<Rigidbody2D>().linearVelocityX = 0;
    }

    public static void AddFoodToEat(string foodName, int nbToEat)
    {
        if (foodToEat.ContainsKey(foodName))
        {
            return;
        }

        foodToEat.Add(foodName, nbToEat);
    }

    public static void ResetFoodToEat()
    {
        foodToEat.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // cannot eat anymore if the game is already won
        if (gameFinished)
        {
            return;
        }

        // start eat animation
        StartCoroutine("EatAnimation");

        // add +1 to eaten food
        string foodName = collision.GetComponent<SpriteRenderer>().sprite.name;
        eatenFood[foodName] += 1;
        eatenFoodTexts[foodName].text = (int.Parse(eatenFoodTexts[foodName].text) + 1).ToString();

        // check if player won or lost current level
        CheckWinLoose();

        // destroy eaten food
        Destroy(collision.gameObject);
    }

    private IEnumerator EatAnimation()
    {
        isEating = true;
        bool tmpIsHoping = GetComponent<Animator>().GetBool("isHoping");
        GetComponent<Animator>().SetBool("isHoping", true);
        yield return new WaitForSeconds(0.3f);
        FindFirstObjectByType<AudioManager>().Play("BunnyCrunch");
        GetComponent<Animator>().SetBool("isHoping", tmpIsHoping);
        isEating = false;
    }

    private void CheckWinLoose()
    {
        bool won = true;
        foreach (string foodName in foodToEat.Keys)
        {
            if (eatenFood[foodName] != foodToEat[foodName])
            {
                won = false;
            }

            if (eatenFood[foodName] > foodToEat[foodName])
            {
                // lost
                gameFinished = true;
                StartCoroutine("Defeat");
                return;
            }
        }

        if (won)
        {
            // won
            gameFinished = true;
            StartCoroutine("Victory");
        }
    }

    private IEnumerator Victory()
    {
        // Start victory animation
        FindFirstObjectByType<AudioManager>().StopAll();
        FindFirstObjectByType<AudioManager>().Play("Victory");
        victoryAnimation.SetActive(true);

        // no skip for 1s
        yield return new WaitForSeconds(1);

        // wait for player touch
        while (Input.touches.Length != 1 || Input.touches[0].phase != TouchPhase.Began)
        {
            yield return null;
        }

        // earn bunny stars
        ShopManager.bunnyStars += LevelManager.level;
        SaveManager.Save();

        // change scene
        SceneManager.LoadScene("LevelTransition");
    }

    public IEnumerator Defeat()
    {
        // Start defeat animation
        FindFirstObjectByType<AudioManager>().StopAll();
        FindFirstObjectByType<AudioManager>().Play("EvilLaugh");
        defeatBackground.SetActive(true);
        defeatAnimation.SetActive(true);
        GameObject.Find("Devil").SetActive(false);

        // no skip for 1s
        yield return new WaitForSeconds(1);

        // wait for player touch
        while (Input.touches.Length != 1 || Input.touches[0].phase != TouchPhase.Began)
        {
            yield return null;
        }

        // show earned bunny stars
        FindFirstObjectByType<AudioManager>().StopAll();
        FindFirstObjectByType<AudioManager>().Play("DigitalCounter");
        defeatAnimation.SetActive(false);
        bunnyStarEarningBackground.SetActive(true);
        bunnyStarEarningText.gameObject.SetActive(true);
        int earnedBunnyStars = (LevelManager.level - 1) * LevelManager.level / 2; // 1+2+3+...+n where n is LevelManager.level - 1
        for (int bunnyStarsIndex = ShopManager.bunnyStars - earnedBunnyStars; bunnyStarsIndex <= ShopManager.bunnyStars; bunnyStarsIndex++)
        {
            bunnyStarEarningText.text = bunnyStarsIndex.ToString();
            yield return new WaitForSeconds(0.1f);
        }
        FindFirstObjectByType<AudioManager>().StopAll();

        // wait for player touch
        while (Input.touches.Length != 1 || Input.touches[0].phase != TouchPhase.Began)
        {
            yield return null;
        }

        // type lolo text
        bunnyStarEarningBackground.SetActive(false);
        yield return TypeText(loloText);

        // wait for player touch
        while (Input.touches.Length != 1 || Input.touches[0].phase != TouchPhase.Began)
        {
            yield return null;
        }

        // change scene
        SceneManager.LoadScene("MiniGameMenu");
    }

    private IEnumerator TypeText(Text text)
    {
        // activate game object
        text.gameObject.SetActive(true);

        // store text to display and erease it
        string textToDisplay = text.text;
        text.text = "";

        // split text into paragraphs
        string[] paragraphs = textToDisplay.Split(".\n");
        foreach (string paragrah in paragraphs)
        {
            // display a paragraph
            FindFirstObjectByType<AudioManager>().Play("Typing");
            foreach (char c in paragrah.ToCharArray())
            {
                text.text += c;
                yield return new WaitForSeconds(0.03f);
            }
            FindFirstObjectByType<AudioManager>().StopAll();

            // wait a bit before next paragraph, if there is one
            if (paragrah == paragraphs[paragraphs.Length - 1])
            {
                break;
            }
            for (int i = 0; i < 3; i++)
            {
                text.text += ".";
                yield return new WaitForSeconds(0.3f);
                text.text = text.text.Remove(text.text.Length - 1);
                yield return new WaitForSeconds(0.3f);
            }
            text.text += ".\n";
        }
    }
}
