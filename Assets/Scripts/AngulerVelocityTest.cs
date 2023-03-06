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
        //_rb.angularVelocity = _angulerVelocity;
        //Debug.DrawRay(transform.position + transform.forward, CaterpillarWheel.PointVelocity(_rb.angularVelocity, transform.forward));
        //Debug.DrawRay(transform.position + transform.forward * 2, CaterpillarWheel.PointVelocity(_rb.angularVelocity, transform.forward * 2));
        //Debug.DrawRay(transform.position + transform.forward * 3, CaterpillarWheel.PointVelocity(_rb.angularVelocity, transform.forward * 3));
        //Debug.DrawRay(transform.position + transform.right, CaterpillarWheel.PointVelocity(_rb.angularVelocity, transform.right));
        //Debug.DrawRay(transform.position + transform.up, CaterpillarWheel.PointVelocity(_rb.angularVelocity, transform.up));
        //Debug.DrawRay(transform.position - transform.forward, CaterpillarWheel.PointVelocity(_rb.angularVelocity, -transform.forward));
        //Debug.DrawRay(transform.position - transform.right, CaterpillarWheel.PointVelocity(_rb.angularVelocity, -transform.right));
        //Debug.DrawRay(transform.position - transform.up, CaterpillarWheel.PointVelocity(_rb.angularVelocity, -transform.up));
        //Debug.Log(CaterpillarWheel.PointVelocity(_rb.angularVelocity, transform.forward).magnitude);
        //Debug.Log(CaterpillarWheel.PointVelocity(_rb.angularVelocity, transform.forward * 2).magnitude);
        //Debug.Log(CaterpillarWheel.PointVelocity(_rb.angularVelocity, transform.forward * 3).magnitude);
    }
}
