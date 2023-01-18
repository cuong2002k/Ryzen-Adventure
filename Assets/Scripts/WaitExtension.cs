using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public static class WaitExtension
{
    public static void Wait(this MonoBehaviour mono, float timeDelay, UnityAction action)
    {
        mono.StartCoroutine(ExcuteAction(timeDelay, action));
    }
    private static IEnumerator ExcuteAction(float timeDelay, UnityAction action)
    {
        yield return new WaitForSeconds(timeDelay);
        action.Invoke();
        yield break;
    }
}
