using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private readonly float _speedPlayer = 10f;
    //private readonly float _jumpForce = 15f;
    [SerializeField]private float _jumpForce = 15f;
    private Rigidbody2D _playerRB;
    private PlayerInput _playerInput;
    private PlayerCollision _playerColl;
    private PlayerAnimationController _playerAmin;

    private void Start()
    {
        _playerRB = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _playerColl = GetComponent<PlayerCollision>();
        _playerAmin = GetComponent<PlayerAnimationController>();
    }

    private void FixedUpdate()
    {

        Movement();
        Jumping();

    }

    private void Movement()
    {
        Vector2 target = new Vector2(_speedPlayer * _playerInput.MoveDirection, _playerRB.velocity.y);
        _playerRB.velocity = target;
    }

    private void Jumping()
    {
        if (_playerInput.IsJumping && _playerColl.IsGrounded)
        {
            Vector2 target = new Vector2(_playerRB.velocity.x, _jumpForce);
            _playerRB.velocity = target;
        }
    }
}
