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
                Vector3 velocity = _rb.GetPointVelocity(hit.point);
                Vector3 force = hit.normal * Mathf.Min(0, -Vector3.Dot(hit.normal, velocity));
                Debug.DrawRay(hit.point, _rb.GetPointVelocity(hit.point), Color.red);
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

    /// <summary>
    /// RigidbodyñàÇÃCaterpillarWheelÇÃèWÇ‹ÇË
    /// </summary>
    class WheelGorup
    {

        public WheelGorup(CaterpillarWheel wheels, Rigidbody rigidbody)
        {
            _wheels.Add(wheels, false);
            _rigidbody = rigidbody;
        }

        public List<CaterpillarWheel> Wheels { get => _wheels.Keys.ToList(); }
        public Rigidbody Rigidbody { get => _rigidbody; }

        Dictionary<CaterpillarWheel, bool> _wheels = new Dictionary<CaterpillarWheel, bool>();
        Rigidbody _rigidbody;

        public bool AddWheel(CaterpillarWheel wheel)
        {
            if (_rigidbody != wheel.Body) { return false; }
            if(_wheels.Keys.Contains(wheel)) { return false; }

            _wheels.Add(wheel, false);

            return true;
        }

        public void Remove(CaterpillarWheel wheel)
        {
            _wheels.Remove(wheel);
        }
    }
}

