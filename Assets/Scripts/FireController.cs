using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    protected Transform _stockTransform;
    // Start is called before the first frame update
    protected void Start()
    {
        _stockTransform = new GameObject().transform;
        _stockTransform.name = $"{name}Bullets";
    }
}
