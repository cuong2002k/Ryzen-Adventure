using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeal : MonoBehaviour
{
    private PlayerAnimationController _playerAminsc;
    private int _healPoint; // current hp

    private readonly float _immortalTime = 2f; // time Immortal (time bat tu)
    private float _curImmortalTime = 0f;
    private int _maxHealPoint { get; set; } = 100; //max hp
    [SerializeField] private ParticleSystem _hurtEffect;

    private void Start()
    {
        _healPoint = _maxHealPoint;
        _playerAminsc = GetComponent<PlayerAnimationController>();
    }

    private void Update()
    {
        if (_curImmortalTime >= 0 && !_playerAminsc.AirAttack) _curImmortalTime -= Time.deltaTime;

    }

    public void TakeDame(int outDamage)
    {
        if (_playerAminsc.AirAttack)
        {
            _curImmortalTime = _immortalTime - 1f;
            return;
        }

        if (_curImmortalTime <= 0)
        {
            _healPoint -= outDamage;
            if (_healPoint <= 0)
            {
                _playerAminsc.Die();
                Debug.Log("Die");
            }
            else
            {
                _playerAminsc.Hit();
                _curImmortalTime = _immortalTime;
                Debug.Log(_healPoint);
            }
            _hurtEffect.Play();
        }

    }

}
