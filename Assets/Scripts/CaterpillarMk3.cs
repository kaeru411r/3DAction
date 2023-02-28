using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CaterpillarMk3 : MonoBehaviour
{
    Rigidbody _rb;
    [SerializeField] CaterpillarWheel _weel;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        for (int i = 0; i < 4; i++)
        {
            for (int k = 0; k < 4; k++)
            {
                Vector3 pos = transform.position + transform.right * (i - 1.5f) + transform.forward * (k - 1.5f) + -transform.up;
                Instantiate(_weel, pos, Quaternion.identity, transform);
            }
        }
    }

    private void FixedUpdate()
    {
    }
}