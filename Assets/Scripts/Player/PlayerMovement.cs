using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D _playerRb;
    private Animator _playerAnimation;

    [Space]
    [Header("Movement")]
    private float _movementAccelration = 70f; // gia tốc
    private float _maxMoveSpeed = 16f;   // tốc độ di chuyển tối đa
    [SerializeField] private float _groundlinearDrag = 10f; // hệ số ma sát chuyển động
    private bool _changeDirection => (_horizontal < 0 && _playerRb.velocity.x > 0)
                                    || (_horizontal > 0 && _playerRb.velocity.x < 0);
    private float _horizontal;
    private float _vertical;
    private bool _isFacingRight = true;
    private bool _isWalk;

    [SerializeField] private PlayerCollision _PlayerCollision;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private bool _isTouchingWall;


    [Space]
    [Header("Jumping")]
    [SerializeField, Range(0, 50)] private float _jumpforce = 30f;
    [SerializeField, Range(0, 20)] private float _fallMultiplier = 6f;
    [SerializeField, Range(0, 20)] private float _lowfallMultiplier = 3f;
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

    [Space]
    [Header("Dash")]
    [SerializeField] private float _dashSpeed = 50f;
    [SerializeField] private float _dragDashLinear = 100f;
    private bool _isDashed = false;
    [SerializeField] private float _dashTimer = 0.1f;
    private bool _canDash = true;


    [Space]
    [Header("Partical")]
    public ParticleSystem dashParticle;
    public ParticleSystem slideParticle;
    public ParticleSystem jumpParticle;
    public ParticleSystem wallJumpParticle;
    [SerializeField] private RippleEffect rippleEffect;
    [SerializeField] private GhostTrail ghostTrail;
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
        //Checkcollision();
        _isGrounded = _PlayerCollision._isGrounded;
        _isTouchingWall = _PlayerCollision._isTouchingWall;
        LinearDrag();
        Movement();
        Jumping();
        Falling();
        WallSliding();
        CheckDashed();
    }

    private void CheckInput()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

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

        if (Input.GetKeyDown(KeyCode.Z) && !_isDashed)
        {
            _isDashed = true;

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
            _playerRb.velocity = new Vector2(_horizontal * _movementAccelration, _playerRb.velocity.y);
        }

        if (Mathf.Abs(_playerRb.velocity.x) > _maxMoveSpeed)
        {
            _playerRb.velocity = new Vector2(_horizontal * _maxMoveSpeed, _playerRb.velocity.y);
        }

        _isWalk = (_playerRb.velocity.x != 0);

    }

    private void LinearDrag()
    {
        if (_isGrounded)
        {
            ApplyGroundLinearDrag();
            _canDash = true;
        }
        else
        {
            ApplyAirLinearDrag();
        }

    }

    private void ApplyGroundLinearDrag()
    {
        if (Mathf.Abs(_horizontal) < 0.4f || _changeDirection)
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
            JumpParticle().Play();

        }
    }

    private void CheckIfWallSliding()
    {
        if (_isWallSliding)
        {
            slideParticle.Play();
        }
        else
        {
            slideParticle.Stop();
        }

        if (!_isGrounded && _isTouchingWall && _playerRb.velocity.y < 0 && _horizontal != 0)
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
            slideParticle.Play();
            if (_playerRb.velocity.y < -_wallSlidingSpeed)
            {
                _playerRb.velocity = new Vector2(_playerRb.velocity.x, -_wallSlidingSpeed);
            }
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



    private void CheckDashed()
    {
        if (_isDashed && _canDash)
        {
            Vector2 direction = new Vector2(_horizontal, _vertical);
            if (direction == Vector2.zero)
            {
                direction = new Vector2(transform.localScale.x, 0f);
            }
            //StartCoroutine(Dash(direction));
            Dash(direction.normalized);
            
            StartCoroutine(StopDash());
        }
    }

    private IEnumerator StopDash()
    {
        CinemachineShake.instance.StartShakeCamera();
        Camera.main.transform.DOComplete();
        ghostTrail.ShowGhost();
        // if (rippleEffect != null)
        // {
        //     rippleEffect.Emit(Camera.main.WorldToViewportPoint(new Vector3(transform.position.x, transform.position.y, 1f)));
        // }
        dashParticle.Play();

        _playerRb.drag = _dragDashLinear;
        _playerRb.gravityScale = 0;

        yield return new WaitForSeconds(_dashTimer);
        dashParticle.Stop();
        _isDashed = false;
        _canDash = false;
    }
    private void RigidbodyDrag(float x)
    {
        _playerRb.drag = x;
    }
    private void Dash(Vector2 direction)
    {
        if (!_isWallSliding)
        {
            _playerRb.velocity = direction * _dashSpeed;

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
        }
        else
        {
            _playerRb.gravityScale = _baseGravityScale;
        }
    }

    private void CheckDirection()
    {
        if (_horizontal != 0)
            FlipDirection(_horizontal > 0);
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

    private ParticleSystem JumpParticle()
    {
        if (_isWallSliding)
        {
            return wallJumpParticle;
        }
        return jumpParticle;
    }
}
