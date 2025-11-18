using UnityEngine;
using UnityEngine.UI;

public class KeyboardManager : MonoBehaviour
{
    public Text output;

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
                        if (output.text.Length > 0)
                        {
                            output.text = output.text.Remove(output.text.Length - 1);
                        }
                        break;

                    case "enter":
                        break;

                    default:
                        output.text += hitKey;
                        break;

                }
            }
        }
    }
}
