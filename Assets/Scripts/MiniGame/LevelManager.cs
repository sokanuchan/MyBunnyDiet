using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static int level = 0;

    public GameObject path;
    public Animator bunnyAnimator;
    public Text firstLevelNb;
    public Text secondLevelNb;

    private bool canStartNextLevel = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FindFirstObjectByType<AudioManager>().StopAll();
        FindFirstObjectByType<AudioManager>().Play("MiniGame - Menu");

        if (level != 0)
        {
            StartCoroutine("WalkToNextLevel");
            firstLevelNb.text = level.ToString();
            secondLevelNb.text = (level + 1).ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // cannot start playing until WalkToNextLevel is finished
        if (!canStartNextLevel)
        {
            return;
        }

        // check if player hits play
        string hitButton = MenuManager.GetHitButton();
        if (hitButton == "Play")
        {
            ChangeLevel();
            StartCoroutine("StartLevel");
            canStartNextLevel = false;
        }

        // back to mini game menu
        if (hitButton == "Back")
        {
            SceneManager.LoadScene("MiniGameMenu");
        }

    }

    public static void Reset()
    {
        // set level to 0
        level = 0;

        // reset bunny food to eat
        BunnyManager.ResetFoodToEat();

        // reset devil pool
        DevilManager.foodItems.Clear();
    }

    private void ChangeLevel()
    {
        level += 1;

        // add new food items on first levels
        if (level >= 1)
        {
            DevilManager.foodItems["Carbs"] = 10;
            BunnyManager.AddFoodToEat("Carbs", 5);
            DevilManager.poolSize = 10;
        }
        if (level >= 2)
        {
            DevilManager.foodItems["Burger"] = 10;
            BunnyManager.AddFoodToEat("Burger", 1);
            DevilManager.poolSize = 20;
        }
        if (level >= 3)
        {
            DevilManager.foodItems["Meat"] = 10;
            BunnyManager.AddFoodToEat("Meat", 5);
            DevilManager.poolSize = 30;
        }
        if (level >= 4)
        {
            DevilManager.foodItems["Vegetable"] = 10;
            BunnyManager.AddFoodToEat("Vegetable", 5);
            DevilManager.poolSize = 40;
        }
        if (level >= 5)
        {
            DevilManager.foodItems["Fat"] = 10;
            BunnyManager.AddFoodToEat("Fat", 3);
            DevilManager.poolSize = 50;
        }
        if (level >= 6)
        {
            DevilManager.attackSpeed = level;
            DevilManager.moveSpeed = level - 3;
            DevilManager.foodFallSpeed = level;
            BunnyManager.moveSpeed = level - 2;
            DevilManager.poolSize = 50;
        }
    }

    private IEnumerator WalkToNextLevel()
    {
        // set flag to false to avoid player from starting level
        canStartNextLevel = false;

        // wait a bit
        yield return new WaitForSeconds(0.3f);

        // start bunny hoping
        bunnyAnimator.SetBool("isHoping", true);

        // move path until next level
        path.GetComponent<Rigidbody2D>().linearVelocityX = -5;
        while (path.transform.position.x > -7)
        {
            yield return null;
        }
        path.transform.position = new Vector3(-7, path.transform.position.y, path.transform.position.z);
        path.GetComponent<Rigidbody2D>().linearVelocityX = 0;

        // stop bunny hoping
        bunnyAnimator.SetBool("isHoping", false);

        // player can now play
        canStartNextLevel = true;
    }

    private IEnumerator StartLevel()
    {
        // do one bunny hop
        bunnyAnimator.SetBool("isHoping", true);
        yield return new WaitForSeconds(0.3f);
        bunnyAnimator.SetBool("isHoping", false);

        // bunny disapears
        FindFirstObjectByType<AudioManager>().StopAll();
        FindFirstObjectByType<AudioManager>().Play("Glitter");
        SpriteRenderer bunnySprite = bunnyAnimator.gameObject.GetComponent<SpriteRenderer>();
        bunnySprite.color = new Color(0, 0, 0, 0.99f);
        while (bunnySprite.color.a > 0)
        {
            bunnySprite.color = new Color(0, 0, 0, Mathf.Pow(bunnySprite.color.a, 2));
            yield return new WaitForSeconds(0.08f);
        }

        // change scene to game
        SceneManager.LoadScene("Game");
    }
}
