using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy
{
    #region Animator
    private readonly string _idle = "EnemyIdle";
    private readonly string _walk = "EnemyWalk";
    private readonly string _run = "EnemyRunning";
    private readonly string _hit = "EnemyHit";
    #endregion Animator

    private Rigidbody2D _enemyRb;
    private Animator _enemyAmin;
    private Transform _movePointTarget;

    private bool _isDead = false;
    private bool _isHit = false;

    protected override void Start()
    {
        base.Start();
        _enemyRb = GetComponent<Rigidbody2D>();
        _enemyAmin = GetComponent<Animator>();
        _movePointTarget = pointA;

    }

    protected override void Update()
    {
        base.Update();
        if (!_isDead && !_isHit)
        {
            Moving();
        }
    }

    protected void Moving()
    {
        _enemyAmin.Play(_run);
        if (Vector2.Distance(this.transform.position, pointA.position) < 0.01f)
        {
            _movePointTarget = pointB;
            Flip(-1);
        }
        if (Vector2.Distance(this.transform.position, pointB.position) < 0.01f)
        {
            _movePointTarget = pointA;
            Flip(1);
        }
        Vector2 target = Vector2.MoveTowards(transform.position, _movePointTarget.position, enemySpeed * Time.deltaTime);
        transform.position = target;
    }

    public override void TakeDamageEnemy(int Damage)
    {
        base.TakeDamageEnemy(Damage);
        _enemyAmin.Play(_hit);
        _isHit = true;
        this.Wait(0.3f, () =>
        {
            _isHit = false;
        });
    }

    protected override void Die()
    {
        base.Die();
        this.Wait(0.3f, () =>
        {
            Destroy(this.gameObject);
        });
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.GetComponent<PlayerController>().TakeDame(dameEnemy);
        }
    }

    void Flip(int direction){
        _enemyAmin.transform.localScale = new Vector3(direction, 1, 1);
    }

}
