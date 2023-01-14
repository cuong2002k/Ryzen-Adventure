using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator _playerAmin;
    private PlayerInput _playerInput;
    private PlayerMovement _playerMove;
    private PlayerCollision _playerColl;
    private Rigidbody2D _playerRB;

    private void Start()
    {
        _playerAmin = GetComponent<Animator>();
        _playerInput = GetComponent<PlayerInput>();
        _playerMove = GetComponent<PlayerMovement>();
        _playerColl = GetComponent<PlayerCollision>();
        _playerRB = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        string correctAnim;
        if (_playerColl.IsGrounded)
        {
            correctAnim = _playerInput.MoveDirection != 0 ?
                                StateAnimation.Move : StateAnimation.Idle;
            ChangeStateAmin(correctAnim);
        }
        else
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
}
