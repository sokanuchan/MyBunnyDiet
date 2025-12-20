using UnityEngine;

public class HighlightOnTouch : MonoBehaviour
{
    private GameObject highlight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // create the highlight
        highlight = Instantiate(gameObject);
        highlight.GetComponent<SpriteRenderer>().material.shader = Shader.Find("GUI/Text Shader");
        highlight.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
        highlight.transform.position = highlight.transform.position + new Vector3(0, 0, -1);

        // Deactivate colliders on highlight
        if (highlight.GetComponent<BoxCollider2D>() != null)
        {
            highlight.GetComponent<BoxCollider2D>().enabled = false;
        }
        if (highlight.GetComponent<CircleCollider2D>() != null)
        {
            highlight.GetComponent<CircleCollider2D>().enabled = false;
        }

        // Deactivate HighlightOnTouch on highlight
        highlight.GetComponent<HighlightOnTouch>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        highlight.SetActive(IsBeingTouched());
    }

    private bool IsBeingTouched()
    {
        // no touch
        if (Input.touches.Length != 1)
        {
            return false;
        }

        // find where the player is currently touching the screen
        Touch touch = Input.touches[0];
        Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

        // check if this object is being touched
        RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);
        return hit.collider && hit.collider.gameObject == gameObject;
    }
}
