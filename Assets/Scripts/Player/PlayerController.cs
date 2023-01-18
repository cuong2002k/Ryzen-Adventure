using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerAnimationController _playerAminsc;
    private CapsuleCollider2D _playerCapsColl;
    private int _healPoint; // current hp

    private readonly float _immortalTime = 2f; // time Immortal (time bat tu)

    private float _curImmortalTime = 0f;
    public int maxHealPoint { get; private set; } = 100; //max hp
    public int playerDame { get; private set; } = 30; // dame

    private void Start()
    {
        _healPoint = maxHealPoint;
        _playerAminsc = GetComponent<PlayerAnimationController>();
        _playerCapsColl = GetComponent<CapsuleCollider2D>();
    }
    private void Update()
    {
        if (_curImmortalTime >= 0)
        {
            _curImmortalTime -= Time.deltaTime;
        }
    }
    public void TakeDame(int outDamage)
    {
        if (_curImmortalTime <= 0)
        {
            _healPoint -= outDamage;
            if (_healPoint <= 0)
            {
                _playerAminsc.Die();

            }
            else
            {
                _playerAminsc.Hit();
                _curImmortalTime = _immortalTime;
                Debug.Log(_healPoint);
            }
        }

    }
}
