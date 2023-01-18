using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacking : MonoBehaviour
{
    #region var import from game engine

    [SerializeField] private Transform _attackPoint; //attack point draw gimoz
    [SerializeField] private LayerMask _enemyLayer; // layer get attack
    #endregion end import

    #region var scripts
    private PlayerController _playerCtrl;
    #endregion

    #region var
    private float _attackRange = 0.7f;
    #endregion



    private void Start()
    {
        _playerCtrl = GetComponent<PlayerController>();
        

    }

    //player attack -> Enemy , Boss, ....
    public void PlayerAttack()
    {
        Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemyLayer);
        foreach (Collider2D enemy in hitEnemy)
        {
            enemy.GetComponent<Enemy>().TakeDamageEnemy(_playerCtrl.playerDame);
        }


    }

    private void OnDrawGizmos()
    {
        if (_attackPoint == null) return;
        Gizmos.DrawSphere(_attackPoint.position, _attackRange);
    }


}



