using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SoundManager : MonoBehaviour
{
    public static Action<AudioClip> HandleSound;
    public static SoundManager instance;
    private AudioSource _audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        // GameObject[] musicObj = GameObject.FindGameObjectsWithTag("GameMusics");
        // if (musicObj.Length > 1)
        // {
        //     Destroy(this.gameObject);
        // }

        _audioSource = GetComponent<AudioSource>();
        HandleSound += PlayerSource;
    }

    public void PlayerSource(AudioClip audioClip)
    {
        _audioSource.PlayOneShot(audioClip);
    }

    private void OnDestroy()
    {
        HandleSound -= PlayerSource;
    }


}
