using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ԃ̑����̃��C���R���|�[�l���g��
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class CaterpillarMk2 : MonoBehaviour
{

    /// <summary>�z�C�[��</summary>
    ChildWheel[] _wheels;
    /// <summary>���W�b�h�{�f�B</summary>
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
