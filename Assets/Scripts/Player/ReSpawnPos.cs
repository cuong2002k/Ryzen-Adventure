using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSpawnPos : MonoBehaviour
{
    GameManager gameManager;
    private void Start()
    {
        transform.position = GameManager.instance.Posplayer;
    }
}
