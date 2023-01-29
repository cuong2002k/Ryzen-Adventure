using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAnimation
{
    
    public static readonly string Idle = "PlayerIdle";
    public static readonly string Move = "PlayerRun";
    public static readonly string Jump = "PlayerJump";
    public static readonly string Fall = "PlayerFall";
    public static readonly string Dash = "PlayerDash";
    public static readonly string Attack = "PlayerAttack";
    public static readonly string Die = "PlayerDie";
    public static readonly string Hit = "PlayerHit";

    #region animation swap weapon
    public static readonly string IdleWeapon = "PlayerIdleWeapon";
    public static readonly string Weapon = "Weapon";
    public static readonly string NoWeapon = "NoWeapon";
    public static readonly string Puch = "PlayerPunch";
    public static readonly string AirAttack = "AirAttack";
    public static readonly string AirPunch = "AirPunch";
    #endregion

}
