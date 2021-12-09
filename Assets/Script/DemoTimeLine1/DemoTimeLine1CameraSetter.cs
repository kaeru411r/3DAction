using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DemoTimeLine1CameraSetter : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _vcam;
    // Start is called before the first frame update
    public void Set()
    {
        var bullet = GameObject.Find("TestBullet Mk3 No.1(Clone)").transform;
        _vcam.LookAt = bullet;
        _vcam.Follow = bullet;
    }

}
