using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ParametersMenu : MonoBehaviour
{
    public Text backupFileText;
    public GameObject resetConfirmation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        backupFileText.text = SerializationManager.savePath + "save_file" + SerializationManager.saveExtension;
    }

    // Update is called once per frame
    void Update()
    {
        // get hit button
        string hitButton = MenuManager.GetHitButton();

        // handle hit button
        switch (hitButton)
        {
            case "Reset":
                // display reset confimation
                DisplayResetConfirmation(true);
                break;
            case "Yes":
                SaveManager.ResetSave();
                StartCoroutine("ResetAnimation");
                DisplayResetConfirmation(false);
                break;
            case "No":
                DisplayResetConfirmation(false);
                break;
        }
    }

    private void DisplayResetConfirmation(bool display)
    {
        resetConfirmation.SetActive(display);
    }

    private IEnumerator ResetAnimation()
    {
        // load animation
        GameObject resetAnimation = Instantiate(Resources.Load<GameObject>("Animations/Reset/ResetAnimation"), new Vector3(0, 0, -1), Quaternion.identity);

        // destroy animation
        Destroy(resetAnimation, resetAnimation.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

        // wait and play sound effect
        yield return new WaitForSeconds(1.2f);
        FindFirstObjectByType<AudioManager>().Play("Glitter");
    }
}
