using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tester : MonoBehaviour
{
    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddForce(-10, 0, 0,ForceMode.Impulse);
    }

}
