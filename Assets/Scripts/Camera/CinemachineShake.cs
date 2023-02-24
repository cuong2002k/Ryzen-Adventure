using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
public class CinemachineShake : MonoBehaviour
{
    // Start is called before the first frame update
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;

    public static Action Event;
    [SerializeField] private float _shakeIntensity = 1f;
    private float _shakeTimer = 0.2f;
    private float _shakeCounter;
    void Start()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        Event += StartShakeCamera;
    }

    private void OnDestroy()
    {
        Event -= StartShakeCamera;
    }
    public void StartShakeCamera()
    {
        cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = _shakeIntensity;
        _shakeCounter = _shakeTimer;
    }

    private void StopShakeCamera()
    {
        cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
        _shakeCounter = 0;
    }

    private void Update()
    {
        if (_shakeCounter > 0)
        {
            _shakeCounter -= Time.deltaTime;
            if (_shakeCounter <= 0) StopShakeCamera();
        }
    }

}
