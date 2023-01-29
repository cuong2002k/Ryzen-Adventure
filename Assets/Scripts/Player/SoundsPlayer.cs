using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip _hit;
    [SerializeField] private AudioClip _punch;
    [SerializeField] private AudioClip _hurt;
    public static SoundsPlayer soundPlayer;
    private AudioSource _audioPlayer;
    private void Awake()
    {
        soundPlayer = this;
    }
    private void Start()
    {
        _audioPlayer = GetComponent<AudioSource>();
    }
    private void ChangeSound(AudioClip sound) => _audioPlayer.PlayOneShot(sound);

    public void SoundWeapon() => ChangeSound(_hit);

    public void SoundPunch() => ChangeSound(_punch);

    public void SoundHurt() => ChangeSound(_hurt);


}
