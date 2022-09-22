using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 試験用標的
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class TestTarget : MonoBehaviour
{
    [SerializeField] Vector3 _velocity;

    Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = _velocity;
        _rb.useGravity = false;
        _rb.drag = 0;
        _rb.angularDrag = 0;
        Debug.Log($"{name} 移動速度{_velocity.normalized}方向に{_velocity.magnitude}");
    }
}
