using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilManager : MonoBehaviour
{
    public GameObject food;

    public static float attackSpeed = 5;
    public static float moveSpeed = 2;
    public static float foodFallSpeed = 5;
    public static int poolSize = 10;
    public static Dictionary<string, int> foodItems = new Dictionary<string, int>();
    private static Dictionary<string, int> foodPool = new Dictionary<string, int>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // fill the food pool
        foodPool = new Dictionary<string, int>(foodItems);

        // start movement
        GetComponent<Rigidbody2D>().linearVelocityX = moveSpeed;
        
        // start attacking
        StartCoroutine("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        // Right boundary
        if (transform.position.x < -1.7)
        {
            GetComponent<Rigidbody2D>().linearVelocityX = moveSpeed;
        }


        // Left boundary
        if (transform.position.x > 1.7)
        {
            GetComponent<Rigidbody2D>().linearVelocityX = -moveSpeed;
        }
    }

    private void SpawnFood()
    {
        // choose food to spawn
        int foodIndex = Random.Range(0, poolSize - 1);
        string foodItemName = "";
        foreach (KeyValuePair<string, int> foodItem in foodPool)
        {
            foodIndex -= foodItem.Value;
            foodItemName = foodItem.Key;
            if (foodIndex <= 0)
            {
                break;
            }
        }
        foodPool[foodItemName] -= 1;
        poolSize -= 1;

        // spawn the food
        FindFirstObjectByType<AudioManager>().Play("DevilAttack");
        GameObject foodSpawned = Instantiate(food);
        foodSpawned.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/MiniGame/Food/" + foodItemName);
        foodSpawned.transform.position = food.transform.position;
        foodSpawned.GetComponent<Rigidbody2D>().linearVelocityY = -foodFallSpeed;

        // delete food after they passed the sceen
        Destroy(foodSpawned, 5);
    }

    private IEnumerator Attack()
    {
        // attack cooldown
        yield return new WaitForSeconds(5 / attackSpeed);

        // attack animation
        float tmpLinearVelocityX = GetComponent<Rigidbody2D>().linearVelocityX;
        GetComponent<Rigidbody2D>().linearVelocityX = 0;
        GetComponent<Animator>().SetBool("IsAttacking", true);
        yield return new WaitForEndOfFrame();
        GetComponent<Animator>().SetBool("IsAttacking", false);
        yield return new WaitForSeconds(0.4f);
        GetComponent<Rigidbody2D>().linearVelocityX = tmpLinearVelocityX;

        // spawn food
        SpawnFood();

        // attack again
        if (poolSize <= 0)
        {
            yield return new WaitForSeconds(4);
            FindFirstObjectByType<BunnyManager>().StartCoroutine("Defeat");
        }
        StartCoroutine("Attack");
    }
}
