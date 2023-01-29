using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region BaseEnemy
    [SerializeField] protected private string enemyName; // name of enemy
    [SerializeField] protected private float enemySpeed; // enemy speed
    [SerializeField] protected private int healEnemy; //heal enemy
    [SerializeField] protected private int maxHealEnemy;
    [SerializeField] public int dameEnemy;
    protected Rigidbody2D _enemyRb;

    #endregion


    #region EFFECT
    [SerializeField] private ParticleSystem _hitEffect;
    #endregion

    protected virtual void Start()
    {

        healEnemy = maxHealEnemy;
        _enemyRb = GetComponent<Rigidbody2D>();
    }


    public virtual void TakeDamageEnemy(int Damage, Transform playerPos)
    {

        healEnemy -= Damage;
        float timeDelay = 0.5f;
        _hitEffect.Play();
        if (healEnemy <= 0)
        {
            Invoke("Die", timeDelay);
            Die();
        }

    }

    protected virtual void Die()
    {

    }
    protected void KnockBack(Transform playerPos)
    {
        float knockBackForce = 5f;
        Vector2 targetKnockBack = transform.position - playerPos.position;
        _enemyRb.AddForce(targetKnockBack.normalized * knockBackForce, ForceMode2D.Impulse);
    }
}
