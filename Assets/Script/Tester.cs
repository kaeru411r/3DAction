using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Tester : MonoBehaviour
{
    void Update()
    {
        //Debug.Log(transform.eulerAngles);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x-15, transform.eulerAngles.y, transform.eulerAngles.z);
    }

}
