using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy
{
    #region Animator

    private readonly string _walk = "EnemyWalk";
    private readonly string _hurt = "EnemyHit";

    #endregion Animator

    #region 
    private const string _LEFT = "left";
    private const string _RIGHT = "right";
    private string _facingDirection;
    [SerializeField] private float _castLength;
    [SerializeField] private Transform _castPosition; // get positon casting with Ground and wall
    private Vector3 _baseScale;
    #endregion
    private Animator _boarAmin;
    private bool _isDead = false;
    private bool _isHurt = false;
    private string _currentState;
    private float _AminDelay = 0.35f;
    protected override void Start()
    {

        base.Start();
        _facingDirection = _RIGHT;
        _baseScale = transform.localScale;
        _boarAmin = GetComponent<Animator>();

    }

    private void FixedUpdate()
    {
        if (!_isDead && !_isHurt)
        {
            Moving();
            if (CheckGround() || !CheckNear())
            {
                if (_facingDirection == _LEFT) ChangeScale(_RIGHT);
                else if (_facingDirection == _RIGHT) ChangeScale(_LEFT);
            }
        }
    }

    protected void Moving()
    {
        float MoveX = enemySpeed;
        if (_facingDirection == _LEFT) MoveX = -enemySpeed;
        _enemyRb.velocity = new Vector2(MoveX, _enemyRb.velocity.y);
        ChangeAnimation(_walk);
    }

    private bool CheckGround()
    {
        bool isGround = false;
        string direction = _facingDirection;
        float targetLengCast = _castLength;

        if (direction == _LEFT)
        {
            targetLengCast = -targetLengCast;
        }

        Vector3 targetPosCast = _castPosition.position;
        targetPosCast.x += targetLengCast;

        Debug.DrawLine(_castPosition.position, targetPosCast, Color.red);
        if (Physics2D.Linecast(_castPosition.position, targetPosCast, 1 << LayerMask.NameToLayer("IsGround")))
        {
            isGround = true;
        }

        return isGround;
    }

    private bool CheckNear()
    {
        bool isGround = false;
        string direction = _facingDirection;
        float targetLengCast = _castLength;
        Vector3 targetPosCast = _castPosition.position;
        targetPosCast.y -= targetLengCast;
        Debug.DrawLine(_castPosition.position, targetPosCast, Color.blue);
        if (Physics2D.Linecast(_castPosition.position, targetPosCast, 1 << LayerMask.NameToLayer("IsGround")))
        {
            isGround = true;
        }

        return isGround;
    }

    private void ChangeScale(string newDirection)
    {
        Vector3 newScale = _baseScale;
        if (newDirection == _RIGHT)
        {
            newScale.x = _baseScale.x;
        }
        else if (newDirection == _LEFT)
        {
            newScale.x = -_baseScale.x;
        }

        transform.localScale = newScale;
        _facingDirection = newDirection;

    }

    public override void TakeDamageEnemy(int Damage, Transform playerPos)
    {

        base.TakeDamageEnemy(Damage, playerPos);
        ChangeAnimation(_hurt);
        _isHurt = true;
        KnockBack(playerPos);
        this.Wait(_AminDelay, () => { _isHurt = false; });

    }

    protected override void Die()
    {

        base.Die();
        this.Wait(_AminDelay, () => { Destroy(this.transform.root.gameObject); });

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.GetComponent<PlayerHeal>().TakeDame(dameEnemy);
        }

    }

    private void ChangeAnimation(string state)
    {

        if (state == _currentState) return;
        _boarAmin.Play(state);
        _currentState = state;

    }

}
