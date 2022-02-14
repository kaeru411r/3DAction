using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class Tester : MonoBehaviour
{
     Rigidbody r;

    private void OnEnable()
    {
        if (!r)
        {
            r = GetComponent<Rigidbody>();
        }
        ExplosionManager.Instance.Add(r);
    }
    private void OnDisable()
    {
        ExplosionManager.Instance.Remove(r);
    }
}