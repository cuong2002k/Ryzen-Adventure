using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    #region Script import

    private Animator _playerAmin;
    private PlayerInput _playerInput;
    private PlayerMovement _playerMove;
    private PlayerCollision _playerColl;
    private PlayerAttacking _playerAttack;
    #endregion

    #region Component

    private Rigidbody2D _playerRB;

    #endregion

    #region var
    private bool _isAtack = false; // check input attack
    private bool _isHit = false;
    private bool _isDead = false;
    private bool _isRun = false;
    private bool _isweapon = false;
    private bool _dontTimeSwapWeapon = false;
    private int _coutAttack = 0; // cout attack input
    private float _maxTimeComboDelay = 0.9f; // time delay combo 
    private float _lastClickTime = 0; // last click when press attack
    #endregion

    private void Start()
    {
        _playerAmin = GetComponent<Animator>();
        _playerInput = GetComponent<PlayerInput>();
        _playerMove = GetComponent<PlayerMovement>();
        _playerColl = GetComponent<PlayerCollision>();
        _playerRB = GetComponent<Rigidbody2D>();
        _playerAttack = GetComponent<PlayerAttacking>();

    }

    private void Update()
    {
        if (Time.time - _lastClickTime > _maxTimeComboDelay)
        {
            _coutAttack = 0;
        }

        if (_playerInput.IsAttack && !_isAtack && _playerColl.IsGrounded)
        {
            _isAtack = true;
            StartCombo();
        }
        _isRun = _playerColl.IsGrounded && !_isAtack && !_isHit && !_isDead;
    }

    private void FixedUpdate()
    {

        string correctAnim;
        if (_playerInput.SwapWeapon)
        {
            _isweapon = !_isweapon;
            correctAnim = (_isweapon) ? StateAnimation.Weapon : StateAnimation.NoWeapon;
            _dontTimeSwapWeapon = true;
            ChangeStateAmin(correctAnim);
            this.Wait(0.35f, () =>
            {
                _dontTimeSwapWeapon = false;
            });
        }

        if (_isRun && !_dontTimeSwapWeapon)
        {
            correctAnim = _playerInput.MoveDirection != 0 ? StateAnimation.Move :
                        (_isweapon == false) ? StateAnimation.Idle : StateAnimation.IdleWeapon;
            ChangeStateAmin(correctAnim);
        }


        if (!_playerColl.IsGrounded)
        {
            correctAnim = _playerRB.velocity.y > .1f ? StateAnimation.Jump : StateAnimation.Fall;
            ChangeStateAmin(correctAnim);
        }
        _playerAmin.SetFloat("MoveX", _playerInput.MoveDirection);

    }

    private void ChangeStateAmin(string state)
    {
        _playerAmin.Play(state);
    }

    private void StartCombo()
    {
        _lastClickTime = Time.time;
        _coutAttack += 1;
        if (_coutAttack > 3)
        {
            _coutAttack = 1;

        }
        ChangeStateAmin(StateAnimation.Attack + _coutAttack);
        _playerAttack.PlayerAttack();

        this.Wait(0.45f, () =>
        {
            _isAtack = false;
        });
    }

    public void Hit()
    {
        ChangeStateAmin(StateAnimation.Hit);
        _isHit = true;
        this.Wait(0.5f, () =>
        {
            _isHit = false;
        });
    }

    public void Die()
    {
        ChangeStateAmin(StateAnimation.Die);
        _isDead = true;
    }
}
