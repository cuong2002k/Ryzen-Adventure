using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMleeAttack : MonoBehaviour
{
    public float distance { get; private set; }
    [SerializeField] private Transform _transformPlayer;


    #region Melee attack
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private LayerMask _playerMask;
    public LayerMask GetplayerMask { get; private set; }
    [SerializeField][Range(0, 1f)] private float _attackRange = 0.5f;
    #endregion

    public readonly float attackDistance = 1f; // distance attacking
    private float _cooldownAttack = 3f; // cool down time attack
    private float _currentHitTime = 0f; // time current cool down


    private void Start()
    {
        GetplayerMask = _playerMask;
    }

    private void Update()
    {
        if (_currentHitTime > 0) _currentHitTime -= Time.deltaTime;
    }

    public bool CheckCooldownAttack()
    {
        if (_currentHitTime <= 0) // check cooldown && distance
        {
            RestartCooldown();// restart cooldown
            return true;
        }
        return false;
    }

    public void Attacking(int dameEnemy) // attack off enemy
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _playerMask);
        foreach (Collider2D player in hit)
        {
            player.GetComponent<PlayerHeal>().TakeDame(dameEnemy);
        }
    }

    private void OnDrawGizmos()
    {
        if (_attackPoint == null) return;
        Gizmos.DrawSphere(_attackPoint.position, _attackRange);
    }

    private void RestartCooldown()
    {
        _currentHitTime = _cooldownAttack;
    }
}
