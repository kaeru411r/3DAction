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
                Ray ray = new (pos, transform.up * -2);
                Debug.DrawRay(pos, ray.direction);
                //if(Physics.Raycast(ray, out RaycastHit hit, 2f))
                //{
                //    _rb.AddForceAtPosition(ray.direction * -(1 - Vector3.Distance(pos, hit.point)), pos, ForceMode.Force);
                //}

                RaycastHit[] hits = Physics.BoxCastAll(pos, Vector3.one * 0.5f, -transform.up, transform.rotation, 1f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
                //_rb.velocity = transform.position;
                if (hits.Length > 0)
                {
                    _rb.AddForceAtPosition(transform.up * -(1 - Vector3.Distance(pos, hits[0].point)), pos, ForceMode.Force);
                    Debug.Log($"{transform.up * -(1 - Vector3.Distance(pos, hits[0].point))}, {Time.frameCount}, {i}, {k}");
                }
            }
        }
    }
}
