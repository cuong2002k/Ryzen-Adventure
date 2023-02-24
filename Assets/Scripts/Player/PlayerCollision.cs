using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [Space]
    [Header("Check Collision")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform _wallcheck;
    [SerializeField] private LayerMask _groundLayer;
    [Range(0, 1), SerializeField] private float _groundCheckRadius;
    [Range(0, 1), SerializeField] private float _wallCheckLength;
    public bool _isGrounded;
    public bool _isTouchingWall;

    private void FixedUpdate()
    {
        Checkcollision();
    }
    private void Checkcollision()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);
        bool direction = (transform.parent.localScale.x > 0);
        _isTouchingWall = Physics2D.Raycast(_wallcheck.position, (direction) ? Vector2.right : Vector2.left, _wallCheckLength, _groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        bool direction = (transform.localScale.x > 0);
        Gizmos.DrawLine(_wallcheck.position, new Vector3(_wallcheck.position.x + ((direction) ? _wallCheckLength : -_wallCheckLength), _wallcheck.position.y, _wallcheck.position.z));
    }



}
