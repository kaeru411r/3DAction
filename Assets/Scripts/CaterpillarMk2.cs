using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ԃ̑����̃��C���R���|�[�l���g��
/// </summary>
public class CaterpillarMk2 : MonoBehaviour
{
    [Tooltip("�ԗ��̃��W�b�h�{�f�B")]
    [SerializeField] Rigidbody _rb;

    /// <summary>�z�C�[��</summary>
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
