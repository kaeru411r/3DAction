using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : MonoBehaviour
{
    [SerializeField] float _distance;
    [SerializeField] float _radius;

    Color _gizmosColor = new Color(1, 0, 0, 0.7f);

    const int _segment = 20;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    //private void OnDrawGizmosSelected()
    //{
    //    float line = Mathf.PI * 2 / _segment;
    //    Vector3 pos = transform.position + transform.up * -1 * _distance;
    //    for (float i = 0; i < Mathf.PI * 2; i += line)
    //    {
    //        Vector3 start = pos + _radius * Mathf.Cos(i) * transform.forward + _radius * Mathf.Sin(i) * transform.right;
    //        Vector3 goal = pos + _radius * Mathf.Cos(i + line) * transform.forward + _radius * Mathf.Sin(i + line) * transform.right;
    //        Gizmos.color = _gizmosColor;
    //        Gizmos.DrawLine(start, goal);
    //    }
    //}
}
