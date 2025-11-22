using UnityEngine;
using UnityEngine.SceneManagement;

public class SportMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // no touch selected
        if (Input.touchCount != 1)
        {
            return;
        }

        // find where the player is currently touching the screen
        Touch touch = Input.touches[0];
        Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

        // Release phase
        if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
        {
            RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);
            if (hit.collider != null)
            {
                string hitButton = hit.collider.name;

                switch (hitButton)
                {
                    case "Muscu": case "Walk": case "Cardio":
                        DailyCategoriesMenu.currentDailyCategory = hitButton;
                        SceneManager.LoadScene("KeyboardInput");
                        break;
                    case "Back":
                        SceneManager.LoadScene("DailyInputs");
                        break;

                }
            }
        }
    }
}
