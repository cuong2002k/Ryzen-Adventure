using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D _playerRb;
    private Animator _playerAnimation;

    [Space]
    [Header("Movement")]
    private float _movementAccelration = 70f; // gia tốc
    private float _maxMoveSpeed = 12f;   // tốc độ di chuyển tối đa
    [SerializeField] private float _groundlinearDrag = 10f; // hệ số ma sát chuyển động
    private bool _changeDirection => (_movementInputDirection < 0 && _playerRb.velocity.x > 0)
                                    || (_movementInputDirection > 0 && _playerRb.velocity.x < 0);
    private float _movementInputDirection;
    private bool _isFacingRight = true;
    private bool _isWalk;


    [Space]
    [Header("Check Collision")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform _wallcheck;
    [SerializeField] private LayerMask _groundLayer;
    [Range(0, 1), SerializeField] private float _groundCheckRadius;
    [Range(0, 1), SerializeField] private float _wallCheckLength;
    private bool _isGrounded;
    private bool _isTouchingWall;


    [Space]
    [Header("Jumping")]
    [SerializeField, Range(0, 50)] private float _jumpforce = 50f;
    [SerializeField, Range(0, 20)] private float _fallMultiplier = 12f;
    [SerializeField, Range(0, 20)] private float _lowfallMultiplier = 9f;
    private float _baseGravityScale = 1f;
    private float _airLinearDrag = 2.5f;
    private float _coyoteTimer = 0.2f;
    [SerializeField] private float _coyoteTimerCounter;
    private float _jumpBufferTimer = 0.25f;
    [SerializeField] private float _jumpBufferCouter;
    private float _airMultiplier = 0.5f;
    private bool _canJumping = false;

    [Space]
    [Header("Wall Sliding // wall jumping")]
    private float _wallSlidingSpeed = 2f;
    private bool _isWallSliding = false;




    private void Start()
    {
        _playerRb = GetComponent<Rigidbody2D>();
        _playerAnimation = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckInput();
        UpdateAnimation();
        CheckDirection();
        CheckJumpTimer();
        CheckIfWallSliding();
    }

    private void FixedUpdate()
    {
        LinearDrag();
        Movement();
        Jumping();
        Checkcollision();
        Falling();
        WallSliding();
    }

    private void CheckInput()
    {
        _movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.X))
        {
            _jumpBufferCouter = _jumpBufferTimer;
        }
        else
        {
            if (_jumpBufferCouter > 0) _jumpBufferCouter -= Time.deltaTime;
        }

        if ((_coyoteTimerCounter > 0 || _isWallSliding) && _jumpBufferCouter > 0)
        {
            _canJumping = true;

        }

        if (Input.GetKeyUp(KeyCode.X) && _playerRb.velocity.y > 0)
        {
            _playerRb.velocity = new Vector2(_playerRb.velocity.x, _playerRb.velocity.y * _airMultiplier);
        }

    }

    private void UpdateAnimation()
    {
        _playerAnimation.SetBool("IsWalk", _isWalk);
        _playerAnimation.SetBool("IsGround", _isGrounded);
        _playerAnimation.SetBool("IsWallSliding", _isWallSliding);
        _playerAnimation.SetFloat("Yvelocity", _playerRb.velocity.y);
    }

    private void Movement()
    {

        if (!_isWallSliding)
        {
            _playerRb.velocity = new Vector2(_movementInputDirection * _movementAccelration, _playerRb.velocity.y);
        }

        if (Mathf.Abs(_playerRb.velocity.x) > _maxMoveSpeed)
        {
            _playerRb.velocity = new Vector2(_movementInputDirection * _maxMoveSpeed, _playerRb.velocity.y);
        }

        _isWalk = (_movementInputDirection != 0);
    }

    private void LinearDrag()
    {
        if (_isGrounded)
        {
            ApplyGroundLinearDrag();
        }
        else
        {
            ApplyAirLinearDrag();
        }

    }

    private void ApplyGroundLinearDrag()
    {
        if (Mathf.Abs(_movementInputDirection) < 0.4f || _changeDirection)
        {
            _playerRb.drag = _groundlinearDrag;
        }
        else
        {
            _playerRb.drag = 0;
        }
    }

    private void ApplyAirLinearDrag()
    {
        _playerRb.drag = _airLinearDrag;
    }

    private void Jumping()
    {
        ApplyAirLinearDrag();
        if (_canJumping)
        {
            _canJumping = false;
            _playerRb.velocity = new Vector2(_playerRb.velocity.x, 0f);
            _playerRb.AddForce(Vector2.up * _jumpforce, ForceMode2D.Impulse);
            _coyoteTimerCounter = 0;
            _jumpBufferCouter = 0;
        }
    }

    private void CheckIfWallSliding()
    {
        if (!_isGrounded && _isTouchingWall && _playerRb.velocity.y < 0 && _movementInputDirection != 0)
        {
            _isWallSliding = true;
        }
        else
        {
            _isWallSliding = false;
        }
    }

    private void WallSliding()
    {
        if (_isWallSliding)
        {
            if (_playerRb.velocity.y < -_wallSlidingSpeed)
                _playerRb.velocity = new Vector2(_playerRb.velocity.x, -_wallSlidingSpeed);
        }
    }

    private void CheckJumpTimer()
    {
        if (_isGrounded)
        {
            _coyoteTimerCounter = _coyoteTimer;
        }
        else
        {
            _coyoteTimerCounter -= Time.deltaTime;
        }
    }

    private void Falling()
    {
        if (_playerRb.velocity.y < 0)
        {
            _playerRb.gravityScale = _fallMultiplier;
        }
        else if (_playerRb.velocity.y > 0 && !Input.GetButtonUp("Jump"))
        {
            _playerRb.gravityScale = _lowfallMultiplier;
            _coyoteTimerCounter = 0;
        }
        else
        {
            _playerRb.gravityScale = _baseGravityScale;
        }
    }

    private void CheckDirection()
    {
        if (_movementInputDirection != 0)
            FlipDirection(_movementInputDirection > 0);
    }

    private void FlipDirection(bool checkFacing)
    {
        Vector3 baseScale = transform.localScale;
        if (checkFacing != _isFacingRight)
        {
            baseScale.x *= -1;
            _isFacingRight = !_isFacingRight;
        }
        transform.localScale = baseScale;

    }
    private void Checkcollision()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);
        bool direction = (transform.localScale.x > 0);
        _isTouchingWall = Physics2D.Raycast(_wallcheck.position, (direction) ? Vector2.right : Vector2.left, _wallCheckLength, _groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        bool direction = (transform.localScale.x > 0);
        Gizmos.DrawLine(_wallcheck.position, new Vector3(_wallcheck.position.x + ((direction) ? _wallCheckLength : -_wallCheckLength), _wallcheck.position.y, _wallcheck.position.z));
    }
}
