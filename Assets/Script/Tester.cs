using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Tester : MonoBehaviour
{
    bool a;
    float b;
    int c = 0;
    float time;
    private void Start()
    {
        GetComponent<Rigidbody>().angularVelocity = new Vector3(2 * Mathf.PI, 0, 0);
        time = Time.time;

    }

    private void Update()
    {
        if(!a && b - transform.eulerAngles.x > 180)
        {
            a = true;
        }
        if (a)
        {
            a = false;
            c++;
            Debug.Log(Time.time - time);
            time = Time.time;
        }
        b = transform.eulerAngles.x;
    }
}

