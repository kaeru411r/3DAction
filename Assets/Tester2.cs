using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Tester2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var c = GetComponent<CinemachineVirtualCamera>();
        var t = c.GetCinemachineComponent<CinemachineTransposer>();
        Debug.Log(transform.position);
        t.m_FollowOffset = Vector3.zero;
        Debug.Log(transform.position);
        transform.position = c.Follow.position + t.m_FollowOffset;
        Debug.Log(transform.position);

    }
}
