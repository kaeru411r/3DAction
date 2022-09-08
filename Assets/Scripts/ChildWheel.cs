using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ChildWheel : MonoBehaviour
{
    /// <summary>�ŏ�����</summary>
    static float minRealNumber = 0.0001f;
    /// <summary>FixedUpdate���s�ς݂̃C���X�^���X</summary>
    static Dictionary<ChildWheel, bool> rans = new Dictionary<ChildWheel, bool>();

    [Tooltip("�z�C�[���̒��a")]
    [SerializeField] float _radius = 0.3f;
    //[Tooltip("�z�C�[���̏d��")]
    //[SerializeField] float _mass = 10;
    [Tooltip("�΂˂̋���")]
    [SerializeField] float _power;
    [Tooltip("�΂˂̒���")]
    [SerializeField] float _length;
    [Tooltip("�z�C�[���̍�p�_")]
    [SerializeField] Vector3 _point;
    [Tooltip("�΂˂̒��S"), Range(0, 1)]
    [SerializeField] float _targetPosition = 0.5f;
    [Tooltip("�ڐG���郌�C���[")]
    [SerializeField] LayerMask _layerMask;


    /// <summary>�ԗ��̃��W�b�h�{�f�B</summary>
    Rigidbody _rb;


    static bool AddInstance(ChildWheel instance)
    {
        if (rans.ContainsKey(instance))
        {
            return false;
        }
        else
        {
            rans.Add(instance, false);
            return true;
        }
    }
    static bool RemoveInstance(ChildWheel instance)
    {
        if (rans.ContainsKey(instance))
        {
            rans.Remove(instance);
            return true;
        }
        else
        {
            return false;
        }
    }

    static bool ran(ChildWheel instance)
    {
        if (rans.ContainsKey(instance))
        {
            rans[instance] = false;
            return true;
        }
        return false;
    }

    static void AddForceAtPosition(Vector3 value, Vector3 point)
    {

    }


    private void OnEnable()
    {
        AddInstance(this);
    }

    private void OnDisable()
    {
        RemoveInstance(this);
    }


    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponentInParent<Rigidbody>();
        if (!_rb)
        {
            Debug.LogWarning($"{nameof(Rigidbody)}���A�^�b�`����Ă��܂���");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        Ground();
    }

    private void OnValidate()
    {
        if (_radius < minRealNumber)
        {
            _radius = minRealNumber;
        }
        if (_power < minRealNumber)
        {
            _power = minRealNumber;
        }
        if (_length < 0)
        {
            _length = 0;
        }
        //if (_mass < minRealNumber)
        //{
        //    _mass = minRealNumber;
        //}
    }

    void Ground()
    {
        if (!_rb)
        {
            return;
        }


        Ray ray = new Ray(_point, -transform.up);
        if(Physics.Raycast(ray, out RaycastHit hit, _length, _layerMask))
        {
            
        }
    }

}
