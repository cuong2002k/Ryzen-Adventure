using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapsColliders : MonoBehaviour
{
    [SerializeField] private AudioClip _collectTrap;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject != null)
            {
                SoundManager.HandleSound?.Invoke(_collectTrap);
                Destroy(other.transform.parent.gameObject);
                GameManager.instance.UpdateOnStateGame(GameState.RestartGame);
            }
        }
    }




}
