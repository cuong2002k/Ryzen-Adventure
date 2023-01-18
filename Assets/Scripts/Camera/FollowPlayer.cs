using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    private Transform _targetFollow;
    private float _speedFollow = 2f;
    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(_targetFollow.position.x, _targetFollow.position.y, -10);
        transform.position = Vector3.Slerp(transform.position, newPos , _speedFollow * Time.deltaTime);
    }
}
