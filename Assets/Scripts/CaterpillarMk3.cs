using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CaterpillarMk3 : MonoBehaviour
{
    Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        for(int i = 0; i < 4; i++)
        {
            for(int k = 0; k < 4; k++)
            {
                Vector3 pos = transform.position + transform.right * (i - 1.5f) + transform.forward * (k - 1.5f) + -transform.up;
                Ray ray = new (pos, transform.up * -1);
                Debug.DrawRay(pos, ray.direction);
                if(Physics.Raycast(ray, out RaycastHit hit, 1f))
                {
                    _rb.AddForceAtPosition(ray.direction * -(1 - Vector3.Distance(pos, hit.point)), pos, ForceMode.Force);
                }
            }
        }
    }
}
