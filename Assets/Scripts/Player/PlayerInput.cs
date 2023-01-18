using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public bool IsMoving => MoveDirection != 0;
    public bool IsJumping { get; private set; } = false;
    public float MoveDirection { get; private set; } = 0;
    public bool IsAttack { get; private set; } = false;

    public bool SwapWeapon { get; private set; } = false;

    private void Update()
    {
        MoveDirection = Input.GetAxisRaw("Horizontal");
        IsJumping = Input.GetKey(KeyCode.Space);
        IsAttack = Input.GetKeyDown(KeyCode.J);
        SwapWeapon = Input.GetKeyDown(KeyCode.W);
    }
}
