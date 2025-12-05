using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SquaresController : MonoBehaviour
{
    public static Transform[] squares = new Transform[9];
    private static int[,] currentSquaresState = { 
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
        LoadSquares();
        if (SquaresImageLoader.nbBunnyParts == 8)
        {
            Shuffle(500);
        }
    }

    public void LoadSquares()
    {
        for (int i = 0; i < squares.Length; i++)
        {
            squares[i] = GameObject.Find("Square_" + i.ToString()).transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // no game if bunny is not complete yet
        if (SquaresImageLoader.nbBunnyParts == 8)
        {
            HandleSlidingSquares();
        }

        // get hit button
        string hitButton = MenuManager.GetHitButton();

        // handle hit button
        switch (hitButton)
        {
            case "Back":
                SceneManager.LoadScene("Bunnies");
                break;
        }
    }

    private void HandleSlidingSquares()
    {
        // no square selected
        if (Input.touchCount != 1)
        {
            selectedSquare = null;
            return;
        }

        // find where the player is currently touching the screen
        Touch touch = Input.touches[0];
        Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

        // new selected square
        if (touch.phase == TouchPhase.Began)
        {
            SelectSquare(touchPos);
        }

        // Moving the square
        if (touch.phase == TouchPhase.Moved && selectedSquare != null && direction != Vector3.zero)
        {
            MoveSquare(touchPos);
        }

        // Release phase
        if ((touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended) && selectedSquare != null && direction != Vector3.zero)
        {
            ReleaseSquare();
        }
    }

    void SelectSquare(Vector3 touchPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);
        if (hit.collider != null && hit.collider.tag == "square")
        {
            selectedSquare = hit.collider.transform;
            originPosition = hit.collider.transform.position;
            direction = GetNearEmptySquareDirection();
        }
    }

    void MoveSquare(Vector3 touchPos)
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

    void ReleaseSquare()
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

        // no empty square near selected square
        return Vector3.zero;
    }

    public static void Shuffle(int shuffleCount)
    {
        int empty_square_y = 2;
        int empty_square_x = 2;

        while (shuffleCount > 0)
        {
            int direction = Random.Range(0, 4);

            // up
            if (direction == 0)
            {
                if (empty_square_y > 0)
                {
                    currentSquaresState[empty_square_y, empty_square_x] = currentSquaresState[empty_square_y - 1, empty_square_x];
                    currentSquaresState[empty_square_y - 1, empty_square_x] = 8;
                    SwapSquares(currentSquaresState[empty_square_y, empty_square_x], currentSquaresState[empty_square_y - 1, empty_square_x]);
                    empty_square_y--;
                    shuffleCount--;
                    continue;
                }
            }

            // down
            if (direction == 1)
            {
                if (empty_square_y < 2)
                {
                    currentSquaresState[empty_square_y, empty_square_x] = currentSquaresState[empty_square_y + 1, empty_square_x];
                    currentSquaresState[empty_square_y + 1, empty_square_x] = 8;
                    SwapSquares(currentSquaresState[empty_square_y, empty_square_x], currentSquaresState[empty_square_y + 1, empty_square_x]);
                    empty_square_y++;
                    shuffleCount--;
                    continue;
                }
            }

            // left
            if (direction == 2)
            {
                if (empty_square_x > 0)
                {
                    currentSquaresState[empty_square_y, empty_square_x] = currentSquaresState[empty_square_y, empty_square_x - 1];
                    currentSquaresState[empty_square_y, empty_square_x - 1] = 8;
                    SwapSquares(currentSquaresState[empty_square_y, empty_square_x], currentSquaresState[empty_square_y, empty_square_x - 1]);
                    empty_square_x--;
                    shuffleCount--;
                    continue;
                }
            }

            // right
            if (direction == 3)
            {
                if (empty_square_x < 2)
                {
                    currentSquaresState[empty_square_y, empty_square_x] = currentSquaresState[empty_square_y, empty_square_x + 1];
                    currentSquaresState[empty_square_y, empty_square_x + 1] = 8;
                    SwapSquares(currentSquaresState[empty_square_y, empty_square_x], currentSquaresState[empty_square_y, empty_square_x + 1]);
                    empty_square_x++;
                    shuffleCount--;
                    continue;
                }
            }
        }

        void SwapSquares(int squareIndex_1, int squareIndex_2)
        {
            Vector3 squareValueTmp = squares[squareIndex_1].position;
            squares[squareIndex_1].position = squares[squareIndex_2].position;
            squares[squareIndex_2].position = squareValueTmp;
        }
    }
}
