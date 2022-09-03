using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 戦車の足回りのメインコンポーネント二号
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class CaterpillarMk2 : MonoBehaviour
{

    /// <summary>ホイール</summary>
    ChildWheel[] _wheels;
    /// <summary>リジッドボディ</summary>
    Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _wheels = GetComponentsInChildren<ChildWheel>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
