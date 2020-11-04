using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.UI;

public class T10_AudioManager : MonoBehaviour
{
    public T10_Sound[] sounds;
    public static T10_AudioManager instance;


    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);


        foreach (T10_Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
        }

    }
    private void Start()
    {
        Play("Theme");
    }
    public void Play(string name)
    {
        T10_Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound : " + name + "not found!");
            return;
        }

        s.source.volume = s.volume;
        s.source.Play();
    }
    public void StopPlaying(string sound)
    {
        T10_Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.volume = s.volume;
        s.source.pitch = s.pitch;

        s.source.Stop();
    }
}

