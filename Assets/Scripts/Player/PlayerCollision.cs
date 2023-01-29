using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Transform groundCheck;
    private CircleCollider2D _playerColl2D;
    private readonly float _radius = 0.3f;

    [SerializeField] public bool IsGrounded { get; private set; }
    private void Start()
    {
        _playerColl2D = GetComponent<CircleCollider2D>();
    }
    private void FixedUpdate()
    {
        // IsGrounded = Physics2D.OverlapCircle(groundCheck.position, _radius, groundLayer) != null;
        IsGrounded = Physics2D.BoxCast(_playerColl2D.bounds.center, _playerColl2D.bounds.size, 0f, Vector2.down, _radius, groundLayer);
    }

}
