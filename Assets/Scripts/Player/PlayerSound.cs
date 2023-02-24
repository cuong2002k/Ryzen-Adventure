using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] private PlayerCollision _playerCollision;
    private AudioSource _soundManager;
    [SerializeField] private AudioClip _touchGroundSound;
    private bool _isFlying;

    private void Start()
    {
        _soundManager = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (!_playerCollision._isGrounded)
        {
            _isFlying = true;
        }
        checkSoundGround();

    }

    private void checkSoundGround()
    {
        if (_isFlying && _playerCollision._isGrounded)
        {
            if(!_soundManager.isPlaying)
                _soundManager.PlayOneShot(_touchGroundSound);
            _isFlying = false;
        }
    }



}
