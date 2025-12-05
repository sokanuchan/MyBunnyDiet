using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SquaresImageLoader : MonoBehaviour
{
    public SpriteRenderer[] squares;
    public static int bunnyIndex = 1;
    public static int nbBunnyParts = 8;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(bunnyIndex);
        Debug.Log(nbBunnyParts);
        // load sprite for each square
        for (int i = 0; i < nbBunnyParts; i++)
        {
            squares[i].sprite = Resources.Load<Sprite>("Images/Bunnies/Bunny-" + bunnyIndex.ToString() + "/SplittedPixelArt/sprite_" + i.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
