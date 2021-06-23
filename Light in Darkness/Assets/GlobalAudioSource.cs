using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAudioSource : MonoBehaviour
{
    public static GlobalAudioSource instance;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudioOneShot(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }
}
