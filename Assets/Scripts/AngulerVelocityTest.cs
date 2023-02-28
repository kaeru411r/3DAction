using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AngulerVelocityTest : MonoBehaviour
{
    [SerializeField] Vector3 _angulerVelocity;

    Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _rb.angularVelocity = _angulerVelocity;
        Debug.DrawRay(transform.position + transform.forward, CaterpillarWheel.PointVelocity(_rb.angularVelocity, transform.forward));
    }
}
