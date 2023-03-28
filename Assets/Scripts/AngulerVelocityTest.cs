using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AngulerVelocityTest : MonoBehaviour
{
    [SerializeField] Vector3 _angulerVelocity;

    Rigidbody _rb;

    Vector3[] points = new Vector3[100];

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        for(int i = 0; i < points.Length; i++)
        {
            points[i] = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, _rb.angularVelocity, Color.yellow);
        _rb.angularVelocity = _angulerVelocity;


        foreach(Vector3 point in points)
        {
            Debug.DrawRay(point + transform.position, _rb.GetRelativePointVelocity(point));
        }
    }
    Vector3 PointVelocity(Vector3 anglerVelocity, Vector3 point)
    {
        float rx = Vector2.Distance(new Vector2(point.y, point.z), Vector2.zero) * anglerVelocity.x;
        float ry = Vector2.Distance(new Vector2(point.x, point.z), Vector2.zero) * -anglerVelocity.y;
        float rz = Vector2.Distance(new Vector2(point.y, point.x), Vector2.zero) * anglerVelocity.z;

        Vector2 yz = new Vector2(-point.z, point.y).normalized * rx * Mathf.PI;
        Vector2 xz = new Vector2(-point.z, point.x).normalized * ry * Mathf.PI;
        Vector2 xy = new Vector2(-point.y, point.x).normalized * rz * Mathf.PI;

        Vector3 result = new(xy.x + xz.x, yz.x + xy.y, yz.y + xz.y);

        return result;
    }
}
