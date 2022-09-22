using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����p�W�I
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
        Debug.Log($"{name} �ړ����x{_velocity.normalized}������{_velocity.magnitude}");
    }
}
