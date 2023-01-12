using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumping : MonoBehaviour
{
    public void Jumping(Rigidbody2D _playerRb, float _jumpForce){
        Vector2 target = new Vector2(_playerRb.velocity.x, _jumpForce);
        _playerRb.velocity = target;
    }
}
