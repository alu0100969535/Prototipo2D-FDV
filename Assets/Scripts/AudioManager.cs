using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour {
    
    [SerializeField] private AudioClip audioClipOnSuccess;
    [SerializeField] private AudioClip audioClipOnFail;
    [SerializeField] private AudioClip audioClipOnLose;
    [SerializeField] private AudioClip audioClipOnNewHighScore;
    [SerializeField] private AudioClip audioClipOnLoseHighScore;
    
    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayOnSuccess() {
        audioSource.PlayOneShot(audioClipOnSuccess);
    }

    public void PlayOnFail() {
        audioSource.PlayOneShot(audioClipOnFail);
    }

    public void PlayOnLose() {
        audioSource.PlayOneShot(audioClipOnLose);
    }

    public void PlayOnNewHighScore() {
        audioSource.PlayOneShot(audioClipOnNewHighScore);
    }

    public void PlayOnLoseNewHighScore() {
        audioSource.PlayOneShot(audioClipOnLoseHighScore);
    }
}
