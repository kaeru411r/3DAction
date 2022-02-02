using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tester : MonoBehaviour
{
    [SerializeField] float p;
    [SerializeField] Rigidbody r;
    private void Start()
    {
        GetComponent<Rigidbody>().velocity = Vector3.forward * p;
        r.AddForce(Vector3.forward * p * r.mass, ForceMode.Impulse);
    }
}