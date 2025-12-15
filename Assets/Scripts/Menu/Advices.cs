using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class Advices : MonoBehaviour
{
    public Text adviceText;
    public GameObject adviceImage1;

    private List<string> advices = new List<string>()
    {
        "Ne lis pas tous les conseils d'un coup.\n\nIls seront bien plus utiles si tu en prends un petit nombre, que tu peux essayer d'appliquer directement.",
        "Le fait de voir la boulimie comme quelque chose d'exterieur a toi est associe avec un meilleur taux de remission.\n\nEntraine-toi a changer ta perspective et a voir cela, non pas comme quelque chose qui fait partie de toi et que tu ne controles pas, mais comme une maladie a combattre.",
        "Il reste important de se faire plaisir.\n\nTu ne tiendras pas sur la duree si ce programme ne te plait pas.",
        "La motivation te fera commencer.\n\nLa discipline te fera prendre les bonnes habitudes.\n\nLes bonnes habitudes t'emmeneront au bout du chemin.",
        "La densite calorique est un parametre tres important pour choisir tes aliments.\n\nTu peux manger beaucoup mais equilibre et bien cuisine ;)",
        "Un gros calin <3",
        "Tu peux regarder ta progression dans la fenetre Bunnies.\n\nAu bout de 30 debloques, le programme est fini.\n\nEn plus, ils sont mignons.",
        "Si tu as vraiment faim, mieux vaut t'autoriser un extra volontaire, qu'un craquage non volontaire.",
        "Soit reguliere, la discipline paiera sur la longueur.",
        "Faire du sport peut etre un bon moyen de depenser des calories.\n\nChoisis un sport que tu aimes, car c'est toujours le sport que tu aimes auquel tu passeras le plus de temps.",
        "Lapin !",
        "Bien dormir t'aidera aussi a avoir une bonne hygiene de vie et a reduire les crises.",
        "Mange un peu plus lentement, ca permettra a ton corps de commencer le processus de digestion avant que tu n'aies fini.",
        "Quand tu as une petite faim, bois d'abord. Parfois, ca suffira et tu te rendras compte que tu avais juste soif. Si ca ne suffit pas, tu peux toujours prendre a manger ensuite.",
        "Essaie de ne pas grignotter.\n\nSi tu dois quand meme le faire pour calmer ta faim, privilegie des aliments bons pour ta sante.\n\nLes aliments industriels ne t'apportent aucune calorie utile, et tu auras peut-etre meme encore plus faim 30 minutes apres.",
        "Assure-toi d'avoir toujours le soutient de ton entourage.\n\nIl est tres tres difficile de se sortir de l'obesite seul.",
        "Reduis tes comportements sedentaires:\n\nSe deplacer en voiture\n\nEtre assis/alonge pour lire\n\nregarder une video\n\n...\n\nPense a te lever pour marcher toutes les heure et demi au moins 5 minutes.",
        "Notes les recettes equilibrees que tu aimes !",
        "Assure-toi de rester accompagne par des professionnels de sante\n\nSans suivi, la demarche perd beaucoup de chances d'aboutir.",
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadRandomAdvice();
    }

    // Update is called once per frame
    void Update()
    {
        // get hit button
        string hitButton = MenuManager.GetHitButton();

        // handle hit button
        switch (hitButton)
        {
            case "Next":
                LoadRandomAdvice();
                break;
        }
    }

    private void LoadRandomAdvice()
    {
        // no advice, just an image (1 time out of 20)
        if (Random.Range(0, 20) == 0)
        {
            adviceText.text = "";
            adviceImage1.SetActive(true);
            return;
        }

        // display random advice
        adviceImage1.SetActive(false);
        int adviceIndex = Random.Range(0, advices.Count);
        adviceText.text = advices[adviceIndex];
    }
}
