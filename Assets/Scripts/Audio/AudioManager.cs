using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found :(");
            return;
        }

        if (s.source.isPlaying)
        {
            Debug.Log("Sound " + name + " already playing");
            return;
        }

        s.source.Play();
    }

    public void Stop(string name)
    {

        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found :(");
            return;
        }

        if (!s.source.isPlaying)
        {
            Debug.Log("Sound " + name + " was not playing");
            return;
        }

        s.source.Stop();
    }

    public void StopAll()
    {
        foreach (Sound s in sounds)
        {
            s.source.Stop();
        }
    }

    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        bool a = s.source.isPlaying;

        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found :(");
            return;
        }

        s.source.Pause();
    }

    public void UnPause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found :(");
            return;
        }

        s.source.UnPause();
    }

    public float GetTimer(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found :(");
            return -1;
        }

        return s.source.time;
    }
}
