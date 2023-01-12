using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public string _idle = "PlayerIdle";
    public string _run = "PlayerRun";
    public string _jump = "PlayerJump";
    public string _fall = "PlayerFall";
    public string _dash = "PlayerDash";
    public void FlipAnimation(Animator _playerAmin, float MoveX){
        _playerAmin.SetFloat("MoveX", MoveX);
    }
}
