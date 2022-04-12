using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Tester2 : MonoBehaviour
{
    [SerializeField] Vector3 _v0;
    [SerializeField] Vector3 _v1;
    [SerializeField] Vector3 _v2;
    [SerializeField] Vector3 v0;
    [SerializeField] Vector3 v1;
    [SerializeField] Vector3 v2;

    private void Update()
    {
        v0 = _v0;
        v1 = _v1;
        v2 = _v2;
        Vector3.OrthoNormalize(ref v0, ref v1, ref v2);
        Debug.DrawRay(Vector3.one, _v0.normalized, Color.red);
        Debug.DrawRay(Vector3.one, _v1.normalized, Color.green);
        Debug.DrawRay(Vector3.one, _v2.normalized, Color.blue);
        Debug.DrawRay(Vector3.zero, v0.normalized, Color.red);
        Debug.DrawRay(Vector3.zero, v1.normalized, Color.green);
        Debug.DrawRay(Vector3.zero, v2.normalized, Color.blue);
    }
}
