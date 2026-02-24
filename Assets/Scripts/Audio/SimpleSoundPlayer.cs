using UnityEngine;

public class SimpleSoundPlayer : MonoBehaviour
{
    public string soundName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound()
    {
        FindFirstObjectByType<AudioManager>().Play(soundName);
    }
}
