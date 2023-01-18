using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private float _length, _startPos;
    [Range(0f, 1f)][SerializeField] private float _parallaxFactor;
    [SerializeField] private GameObject _playerFollow;

    void Start()
    {
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
        _startPos = transform.position.x;

    }


    void Update()
    {
        float temp = _playerFollow.transform.position.x * (1 - _parallaxFactor);
        float distance = _playerFollow.transform.position.x * _parallaxFactor;
        Vector3 newPos = new Vector3(_startPos + distance, transform.position.y, transform.position.z);
        transform.position = newPos;

        if (temp > _startPos + _length / 2) _startPos += _length;
        else if (temp < _startPos - _length / 2) _startPos -= _length;
    }
}
