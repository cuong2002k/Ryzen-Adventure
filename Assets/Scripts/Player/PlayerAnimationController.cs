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
    public bool AirAttack { get; private set; } = false;
    private bool _isHurt = false; // hurt
    private bool _isDead = false; // dead
    private bool _isRun = false; // running
    private bool _isweapon = false; // Weapon or punch
    private bool _SwapWeapon = false;
    private string correctAnim; // current amin
    private int _coutAttack = 0; // cout attack input
    private float _maxTimeComboDelay = 1f; // time delay combo 
    private float _lastClickTime = 0; // last click when press attack
    private float _delayAmin = 0.35f;
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
        // swap Weapon
        if (_playerInput.SwapWeapon)
        {
            _isweapon = !_isweapon;
            correctAnim = (_isweapon) ? StateAnimation.Weapon : StateAnimation.NoWeapon;
            _SwapWeapon = true;
            ChangeStateAmin(correctAnim);
            this.Wait(0.35f, () => { _SwapWeapon = false; });
        }

        RestartComboAttack();

        // Combo attack
        if (_playerInput.IsAttack && !_isAtack)
        {
            _isAtack = true;
            _playerAttack.PlayerAttack();
            ComboPlayer();
        }

        //end combo air attack
        if (_coutAttack == 3 && AirAttack && _playerColl.IsGrounded)
        {
            ChangeStateAmin(((_isweapon) ? StateAnimation.AirAttack : StateAnimation.AirPunch) + 3);
            _coutAttack = 0;
            _playerAttack.PlayerAttackBottom();
            this.Wait(_delayAmin, () => { _isAtack = false; AirAttack = false; });
        }

        // check running
        _isRun = _playerColl.IsGrounded && !_isAtack && !_isHurt && !_isDead;

        //restart attack 
        if (_playerInput.IsJumping) _coutAttack = 0;

    }

    private void FixedUpdate()
    {

        if (_isRun && !_SwapWeapon && !AirAttack)
        {
            correctAnim = _playerInput.MoveDirection != 0 ? StateAnimation.Move :
                        (_isweapon == false) ? StateAnimation.Idle : StateAnimation.IdleWeapon;
            ChangeStateAmin(correctAnim);
        }

        if (!_playerColl.IsGrounded)
        {
            if (!_isAtack)
            {
                correctAnim = _playerRB.velocity.y > .1f ? StateAnimation.Jump : StateAnimation.Fall;
                ChangeStateAmin(correctAnim);
            }
        }
        _playerAmin.SetFloat("MoveX", _playerInput.MoveDirection);

    }

    private void ChangeStateAmin(string state)
    {

        _playerAmin.Play(state);

    }

    private void ComboPlayer()
    {
        _lastClickTime = Time.time;
        _coutAttack += 1;
        if (_coutAttack > 3) _coutAttack = 1;
        if (_playerColl.IsGrounded)
            ComboGroundPlayer();
        else ComboAirPlayer();

    }

    private void ComboGroundPlayer()
    {

        if (_isweapon)
        {
            correctAnim = StateAnimation.Attack + _coutAttack; // if isweapon => combo weapon
            SoundsPlayer.soundPlayer.SoundWeapon();
        }
        else
        {
            correctAnim = StateAnimation.Puch + _coutAttack; // if !isweapon => combo punch
            SoundsPlayer.soundPlayer.SoundPunch();
        }
        ChangeStateAmin(correctAnim);
        this.Wait(_delayAmin, () => { _isAtack = false; });

    }

    private void ComboAirPlayer()
    {

        if (_coutAttack < 3)
        {
            float attackJumpForce = 9f;
            correctAnim = (_isweapon == false) ?
                        StateAnimation.AirPunch + _coutAttack : StateAnimation.AirAttack + _coutAttack;
            if (_isweapon == true)
            {
                SoundsPlayer.soundPlayer.SoundWeapon();
            }
            else
            {
                SoundsPlayer.soundPlayer.SoundPunch();
            }
            this.Wait(_delayAmin, () => { _isAtack = false; });
            _playerRB.velocity = new Vector2(_playerRB.velocity.x, attackJumpForce);
        }
        else
        {
            correctAnim = (_isweapon == false) ? StateAnimation.AirPunch : StateAnimation.AirAttack;
            correctAnim += 4;
            AirAttack = true;
        }
        ChangeStateAmin(correctAnim);

    }

    public void Hit()
    {
        ChangeStateAmin(StateAnimation.Hit);
        SoundsPlayer.soundPlayer.SoundHurt();
        _isHurt = true;
        this.Wait(_delayAmin, () => { _isHurt = false; });

    }

    public void Die()
    {
        ChangeStateAmin(StateAnimation.Die);
        _isDead = true;
    }

    private void RestartComboAttack()
    {
        // restart combo attack
        if (Time.time - _lastClickTime > _maxTimeComboDelay) _coutAttack = 0;
    }
}
