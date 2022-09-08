using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 戦車の足回りのメインコンポーネント二号
/// </summary>
public class CaterpillarMk2 : MonoBehaviour
{
    [Tooltip("車両のリジッドボディ")]
    [SerializeField] Rigidbody _rb;

    /// <summary>ホイール</summary>
    ChildWheel[] _wheels;


    // Start is called before the first frame update
    void Start()
    {
        _wheels = GetComponentsInChildren<ChildWheel>();
        _rb = GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }


}
