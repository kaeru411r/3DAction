using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AddForceTest : MonoBehaviour
{
    [SerializeField] ForceMode _forceMode = ForceMode.Force;
    [SerializeField] Vector3 _force;
    Rigidbody _rb;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        _rb.AddForce(_force, _forceMode);
        sb.Append(_rb.velocity + "" + _force * Time.fixedDeltaTime);
        Debug.Log(sb.ToString());
    }
}
