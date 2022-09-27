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

    Rigidbody Rb
    {
        get
        {
            if (!_rb)
            {
                _rb = GetComponent<Rigidbody>();
            }
            return _rb;
        }
    }

    private void Start()
    {
        Rb.velocity = _velocity;
        Rb.useGravity = false;
        Rb.drag = 0;
        Rb.angularDrag = 0;
        Debug.Log($"{name} �ړ����x{_velocity.normalized}������{_velocity.magnitude}");
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            Rb.velocity = _velocity;
            Debug.Log($"{name} �ړ����x{_velocity.normalized}������{_velocity.magnitude}");
        }
    }

#endif
}
