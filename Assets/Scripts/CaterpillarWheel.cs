using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CaterpillarWheel : MonoBehaviour
{
    static List<WheelGorup> _wheelGroups = new();

    [SerializeField] float _length;
    [SerializeField] float _spling;
    [SerializeField] float _shockAbsorber;
    [SerializeField] float _radius;
    [SerializeField] float _mass;
    [SerializeField] Vector3 _center;

    Rigidbody _rb;
    bool _processed = false;

    public Rigidbody Body { get => _rb; set => _rb = value; }
    public float Length { get => _length; set => _length = value; }
    public float Spling { get => _spling; set => _spling = value; }
    public float ShockAbsorber { get => _shockAbsorber; set => _shockAbsorber = value; }
    public float Radius { get => _radius; set => _radius = value; }
    public float Mass { get => _mass; set => _mass = value; }
    public Vector3 Center { get => _center; set => _center = value; }
    public bool Processed { get => _processed; set => _processed = value; }

    private void Start()
    {
        _rb = GetComponentInParent<Rigidbody>();

        var a = transform.parent.GetComponentsInChildren<CaterpillarWheel>();
    }

    void FixedUpdate()
    {
        Vector3 pos = transform.position + _center + transform.up * (_radius);
        Ray ray = new(pos, -transform.up);
        if (Physics.Raycast(ray, out RaycastHit hit, _radius * 2 + _length))
        {
            float distance = Vector3.Distance(pos, hit.point);
            if (distance < _radius * 2)
            {
                Vector3 point = hit.point - pos;
                Vector3 velocity = _rb.velocity + PointVelocity(_rb.angularVelocity, point);
                Vector3 force = hit.normal * -Vector3.Dot(hit.normal, velocity);
                Debug.DrawRay(hit.point, PointVelocity(_rb.angularVelocity, point), Color.red);
                Debug.DrawRay(hit.point, _rb.velocity, Color.blue);
                Debug.DrawRay(hit.point, velocity, Color.magenta);
                Debug.DrawRay(hit.point, force, Color.green);
                //_rb.transform.Translate(_rb.velocity.normalized * -distance, Space.World);
                _rb.AddForceAtPosition(force, point, ForceMode.Acceleration);
                Debug.Log($"{_rb.velocity}, {_rb.angularVelocity}");
            }
            else
            {
                //Debug.Log(1 - (distance - _radius * 2) / _length);
                _rb.AddForceAtPosition(transform.up * (1 - (distance - _radius * 2) / _length) * _spling, hit.point, ForceMode.Force);
            }
        }
        Debug.DrawRay(ray.origin, ray.direction);
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

    struct WheelGorup
    {
        public CaterpillarWheel[] Wheels { get; set; }
        public Rigidbody Rigidbody { get; set; }
    }
}

