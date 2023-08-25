using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    /// <summary>
    /// Method <c>PlaySound</c> plays a sound
    /// </summary>
    /// <param name="clip"></param> the sound to play
    public void PlaySound(AudioClip clip)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
    }
}
