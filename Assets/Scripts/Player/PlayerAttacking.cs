using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacking : MonoBehaviour
{
    private int _playerDame { get; set; } = 30; // dame
    #region var import from game engine

    [SerializeField] private Transform _attackPointDirection; //attack point draw gimoz
    [SerializeField] private Transform _attackPointBottom; //attack point draw gimoz
    [SerializeField] private LayerMask _enemyLayer; // layer get attack
    #endregion end import

    #region var
    [SerializeField] private float _attackRange = 0.7f;
    #endregion




    //player attack -> Enemy , Boss, ....
    public void PlayerAttack()
    {
        Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(_attackPointDirection.position, _attackRange, _enemyLayer);
        foreach (Collider2D enemy in hitEnemy)
        {
            enemy.GetComponent<Enemy>().TakeDamageEnemy(_playerDame, this.transform);
        }
    }

    public void PlayerAttackBottom()
    {
        Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(_attackPointBottom.position, _attackRange * 3, _enemyLayer);
        foreach (Collider2D enemy in hitEnemy)
        {
            enemy.GetComponent<Enemy>().TakeDamageEnemy(_playerDame, this.transform);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_attackPointDirection == null) return;
        Gizmos.DrawWireSphere(_attackPointDirection.position, _attackRange);
        Gizmos.DrawWireSphere(_attackPointBottom.position, _attackRange * 3);
    }


}



