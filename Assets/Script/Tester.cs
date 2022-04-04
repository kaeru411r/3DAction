using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Tester : MonoBehaviour
{
    [SerializeField] float _x;
    [SerializeField] float _y;
    [SerializeField] float _z;



    private void Update()
    {
        transform.Rotate(_x, _y, _z);
    }
}