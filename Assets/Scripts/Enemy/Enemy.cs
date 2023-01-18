using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    protected private string enemyName; // name of enemy
    [SerializeField]
    protected private float enemySpeed; // enemy speed
    [SerializeField]
    protected private int healEnemy; //heal enemy
    [SerializeField]
    protected private int maxHealEnemy;
    [SerializeField]
    protected private Transform pointA;
    [SerializeField]
    protected private Transform pointB;

    [SerializeField]
    public int dameEnemy;

    protected virtual void Start()
    {
        healEnemy = maxHealEnemy;
    }

    protected virtual void Update()
    {

    }

    public virtual void TakeDamageEnemy(int Damage)
    {
        healEnemy -= Damage;
        float timeDelay = 0.5f;
        if (healEnemy <= 0)
        {
            Invoke("Die", timeDelay);
            Die();
        }
    }

    protected virtual void Die()
    {

    }
}
