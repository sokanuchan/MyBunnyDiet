using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SquaresImageLoader : MonoBehaviour
{
    public SpriteRenderer[] squares;
    public static int bunnyIndex = 1;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // load sprite for each square
        for (int i = 0; i < squares.Count(); i++)
        {
            squares[i].sprite = Resources.Load<Sprite>("Images/Bunnies/Bunny-" + bunnyIndex.ToString() + "/SPlittedPixelArt/sprite_" + i.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
