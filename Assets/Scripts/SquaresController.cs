using UnityEngine;

public class SquaresController : MonoBehaviour
{
    public Transform[] squares;
    private int[,] currentSquaresState = { 
        { 0, 1, 2 },
        { 3, 4, 5 },
        { 6, 7, 8 }
    };
    private Transform selectedSquare;
    private Vector3 originPosition;
    private Vector3 direction;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // no square selected
        if (Input.touchCount != 1)
        {
            selectedSquare = null;
            return;
        }
        
        Touch touch = Input.touches[0];
        Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

        // new selected square
        if (touch.phase == TouchPhase.Began) 
        {
            RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);
            if (hit.collider != null && hit.collider.tag == "square")
            {
                selectedSquare = hit.collider.transform;
                originPosition = hit.collider.transform.position;
                direction = GetNearEmptySquareDirection();
            }
        }

        // Moving the square
        if (touch.phase == TouchPhase.Moved && selectedSquare != null && direction != Vector3.zero)
        {
            Vector3 touchDirection = touchPos - originPosition;

            if (touchDirection.x * direction.x <= 0)
            {
                touchDirection = new Vector3(0, touchDirection.y, 0);
            }
            if (touchDirection.x * direction.x > 1)
            {
                touchDirection = new Vector3(direction.x, 0, 0);
            }
            if (touchDirection.y * direction.y <= 0)
            {
                touchDirection = new Vector3(touchDirection.x, 0, 0);
            }
            if (touchDirection.y * direction.y > 1)
            {
                touchDirection = new Vector3(0, direction.y, 0);
            }

            selectedSquare.position = originPosition + touchDirection;
        }

        // Release phase
        if ((touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended) && selectedSquare != null && direction != Vector3.zero)
        {
            if (Vector3.Distance(selectedSquare.position, originPosition) >= 0.5)
            {
                currentSquaresState[-(int)originPosition.y - (int)direction.y, (int)originPosition.x + (int)direction.x] = currentSquaresState[-(int)originPosition.y, (int)originPosition.x];
                currentSquaresState[-(int)originPosition.y, (int)originPosition.x] = 8;
                selectedSquare.position = originPosition + direction;
            }
            else
            {
                selectedSquare.position = originPosition;
            }
            selectedSquare = null;
            originPosition = Vector3.zero;
            direction = Vector3.zero;
        }
    }

    Vector3 GetNearEmptySquareDirection()
    {
        // up
        if (originPosition.y < 0 && currentSquaresState[-(int)originPosition.y - 1, (int)originPosition.x] == 8)
        {
            return new Vector3(0, 1, 0);
        }

        // down
        if (originPosition.y > -2 && currentSquaresState[-(int)originPosition.y + 1, (int)originPosition.x] == 8)
        {
            return new Vector3(0, -1, 0);
        }

        // left
        if (originPosition.x > 0 && currentSquaresState[-(int)originPosition.y, (int)originPosition.x - 1] == 8)
        {
            return new Vector3(-1, 0, 0);
        }

        // right
        if (originPosition.x < 2 && currentSquaresState[-(int)originPosition.y, (int)originPosition.x + 1] == 8)
        {
            return new Vector3(1, 0, 0);
        }

        return Vector3.zero;
    }
}
