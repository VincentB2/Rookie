using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T10_ThemeSong : MonoBehaviour
{
    public AudioClip loopingPart;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = loopingPart;
            audioSource.Play();
        }
    }
}
