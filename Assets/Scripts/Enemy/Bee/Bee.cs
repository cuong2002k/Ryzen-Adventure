using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : Enemy
{
    // Start is called before the first frame update

    private EnemyMleeAttack _enemyMlee;
    private Animator _beeAmin;
    private AttackArea _enemyAttackArea;
    private bool _isHit = false;
    private bool _isHurt = false;
    private bool _isAlive = true;
    private readonly float _delayAmin = 0.35f;
    [SerializeField] private Transform[] _wayPoint;
    private Transform _nextWayPoint;
    private float _nextPointDistance = 0.5f;
    private float _attackDistance = 1f;

    protected override void Start()
    {
        base.Start();
        _beeAmin = GetComponent<Animator>();
        _enemyMlee = GetComponent<EnemyMleeAttack>();
        _enemyAttackArea = GetComponent<AttackArea>();
        _nextWayPoint = _wayPoint[0];
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {

        if (_isAlive && !_isHurt)
        {
            if (_enemyAttackArea.isPlayer)
            {
                MoveToPlayer();
            }
            else
            {
                MovingToPoint();
            }
            ChangeAnimation(StateBeeAmin.Fly);
            float direction = (_enemyRb.velocity.x < 0) ? 1 : -1;
            _beeAmin.SetFloat("MoveX", direction);
        }
    }

    protected override void Die()
    {
        base.Die();
        _isAlive = false;
        this.Wait(_delayAmin, () => { Destroy(this.transform.root.gameObject); });

    }


    public override void TakeDamageEnemy(int Damage, Transform playerPos)
    {
        base.TakeDamageEnemy(Damage, playerPos);
        ChangeAnimation(StateBeeAmin.Hurt);
        _isHurt = true;
        KnockBack(playerPos);
        this.Wait(_delayAmin, () => { _isHurt = false; });
    }

    private void MoveToPlayer()
    {

        Vector2 playePosition = _enemyAttackArea.playerTranform.position; // position of player
        Vector2 beePosition = this.transform.position; // position of enemy
        Vector2 targetPointPlayer = (playePosition - beePosition).normalized;

        float distanceToPlayer = Vector2.Distance(this.transform.position, playePosition);

        if (distanceToPlayer > _attackDistance)
        {
            _enemyRb.velocity = targetPointPlayer * enemySpeed;
        }
        else if (distanceToPlayer <= _attackDistance && _enemyMlee.CheckCooldownAttack())
        {
            _enemyMlee.Attacking(this.dameEnemy);
            ChangeAnimation(StateBeeAmin.Hit);
            _isHit = true;
            this.Wait(_delayAmin, () => { _isHit = false; });
        }

    }

    private void MovingToPoint()
    {
        Vector2 targetWayPoint = (_nextWayPoint.position - transform.position).normalized;
        float distanceWayPoint = Vector2.Distance(transform.position, _nextWayPoint.position);
        _enemyRb.velocity = targetWayPoint * enemySpeed;
        if (distanceWayPoint <= _nextPointDistance)
        {
            _nextWayPoint = _wayPoint[Random.Range(0, _wayPoint.Length)];
        }
    }




    private void ChangeAnimation(string state) => _beeAmin.Play(state);


}
