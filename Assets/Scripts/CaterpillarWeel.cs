using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaterpillarWeel : MonoBehaviour
{
    Rigidbody _rb;
    [SerializeField] float _length;
    [SerializeField] float _spling;
    [SerializeField] float _shockAbsorber;
    [SerializeField] float _radius;
    [SerializeField] float _mass;
    [SerializeField] Vector3 _center;

    public Rigidbody Body { get => _rb; set => _rb = value; }
    public float Length { get => _length; set => _length = value; }
    public float Spling { get => _spling; set => _spling = value; }
    public float ShockAbsorber { get => _shockAbsorber; set => _shockAbsorber = value; }
    public float Radius { get => _radius; set => _radius = value; }
    public float Mass { get => _mass; set => _mass = value; }
    public Vector3 Center { get => _center; set => _center = value; }

    private void Awake()
    {
        _rb = GetComponentInParent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 pos = transform.position + _center + transform.up * (_radius);
        Ray ray = new(pos, -transform.up);
        if (Physics.Raycast(ray, out RaycastHit hit, _radius * 2 + _length))
        {
            float distance = Vector3.Distance(pos, hit.point);
            if (distance < _radius * 2) { }
            else
            {
                Debug.Log(1 - (distance - _radius * 2) / _length);
                _rb.AddForceAtPosition(transform.up * (1 - (distance - _radius * 2) / _length) * _spling, hit.point, ForceMode.Force);
            }
        }
        Debug.DrawRay(ray.origin, ray.direction);
    }
}
