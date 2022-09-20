using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ŽŽŒ±—p•W“I
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class TestTarget : MonoBehaviour
{
    [SerializeField] Vector3 velocity;

    Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = velocity;
        _rb.useGravity = false;
        _rb.drag = 0;
        _rb.angularDrag = 0;
    }
}
