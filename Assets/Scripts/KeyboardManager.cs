using UnityEngine;

public class KeyboardManager : MonoBehaviour
{
    private static string output = "";

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
            if (hit.collider != null && hit.collider.tag == "key")
            {
                string hitKey = hit.collider.name;

                switch (hitKey)
                {
                    case "delete":
                        output = output.Remove(output.Length - 1);
                        break;

                    case "enter":
                        output += "enter";
                        break;

                    default:
                        output += hitKey;
                        break;

                }
            }

            Debug.Log(output);
        }
    }
}
