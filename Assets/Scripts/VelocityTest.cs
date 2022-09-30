using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VelocityTest : MonoBehaviour
{
    [SerializeField] Vector3 _velocity;
    Rigidbody _rb;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = _velocity;
        _rb.useGravity = false;
        _rb.drag = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(transform.position + "" + (transform.position + _velocity * Time.fixedDeltaTime));
    }
}
