using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static string GetHitButton()
    {
        // no touch selected
        if (Input.touchCount != 1)
        {
            return "";
        }

        // find where the player is currently touching the screen
        Touch touch = Input.touches[0];
        Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

        // Check if player just released touch
        if (touch.phase != TouchPhase.Canceled && touch.phase != TouchPhase.Ended)
        {
            return "";
        }

        // get touched object
        RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);
        if (hit.collider == null)
        {
            return "";
        }

        return hit.collider.name;
    }
}
