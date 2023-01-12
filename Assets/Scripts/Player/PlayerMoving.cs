using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    private float _horizontal;
    private float _speed = 7f;
    public void Moving(Rigidbody2D _playerRB, float _horizontal){
        Vector2 target = new Vector2(_speed * _horizontal , _playerRB.velocity.y);
        _playerRB.velocity = target;

    }
}
