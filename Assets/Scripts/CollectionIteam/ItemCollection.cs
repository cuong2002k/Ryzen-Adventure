using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollection : MonoBehaviour
{
    [SerializeField] private int coreItem;
    [SerializeField] private AudioClip _touchItem;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ActivetedSoundItem(_touchItem);
            Destroy(this.gameObject);
        }
    }

    private void ActivetedSoundItem(AudioClip soundItem)
    {
        SoundManager.HandleSound?.Invoke(soundItem);
    }
}
