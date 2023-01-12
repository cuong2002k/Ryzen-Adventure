using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region import Script 
    private PlayerMoving _playerMoving;
    private PlayerJumping _playerJumping;

    private PlayerAnimation _stateAmin;
    #endregion

    #region var 
    private float _jumpForce = 15f;
    private float _horizontal;
    private float dashSpeed = 10f;

    private bool _isJumping = false;
    private bool _isGround = false;
    private bool _isDash = false;
    #endregion

    #region Import Component
    private Rigidbody2D _playerRB;
    private CircleCollider2D _playerBox;

    private Animator _playerAmin;
    #endregion

    public LayerMask _groundMask;
    private void Start() {
        _playerMoving = GetComponent<PlayerMoving>();
        _playerRB = GetComponent<Rigidbody2D>();
        _playerJumping = GetComponent<PlayerJumping>();
        _playerBox = GetComponent<CircleCollider2D>();
        _playerAmin = GetComponent<Animator>();
        _stateAmin = GetComponent<PlayerAnimation>();
    }
    private void Update() {
        //Check is ground
        _isGround = CheckIsGround();
        //moving
        _horizontal = Input.GetAxisRaw("Horizontal");
        if(_horizontal != 0){
            _stateAmin.FlipAnimation(_playerAmin,_horizontal);
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            _isJumping = true;
        }
        if(Input.GetKeyDown(KeyCode.F)){
            _isDash = true;
        }
    }

    private void FixedUpdate() {
         _playerMoving.Moving(_playerRB, _horizontal);
        // if is ground then run or idle
        if (_isGround)
        {
            if (_isDash)
            {
                ChangeAnimation(_stateAmin._dash);
                float timeCurAmin = _playerAmin.GetCurrentAnimatorStateInfo(0).length;
                float direction = _playerAmin.GetFloat("MoveX");
                for(float i = 0; i<= timeCurAmin; i+=Time.deltaTime){

                    _playerRB.velocity = new Vector2(direction * dashSpeed,_playerRB.velocity.y);
                }
                Invoke("DelayDash", timeCurAmin);
            }
            else
            {
                if (_horizontal != 0f )
                {
                    ChangeAnimation(_stateAmin._run);
                }
                else
                {
                    ChangeAnimation(_stateAmin._idle);
                }
            }
        }
        
        // check jumping
        if(_isJumping == true && _isGround){
            _isJumping = false;
            ChangeAnimation(_stateAmin._jump);
           _playerJumping.Jumping(_playerRB , _jumpForce);
        }
        if(!_isGround){
            if(_playerRB.velocity.y < -0.1f){
                ChangeAnimation(_stateAmin._fall);
            }
        }
        
    }
    private bool CheckIsGround(){
        return Physics2D.BoxCast(
            _playerBox.bounds.center,
            _playerBox.bounds.size,
             0f, 
             Vector2.down,
             .1f, 
            _groundMask);
    }
    private void ChangeAnimation(string animation){
        _playerAmin.Play(animation);
    }

    private void DelayDash(){
        _isDash = false;
    }
}
