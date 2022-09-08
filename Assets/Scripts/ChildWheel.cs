using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ChildWheel : MonoBehaviour
{
    /// <summary>最少実数</summary>
    static float minRealNumber = 0.0001f;
    /// <summary>FixedUpdate実行済みのインスタンス</summary>
    static Dictionary<ChildWheel, bool> rans = new Dictionary<ChildWheel, bool>();

    [Tooltip("ホイールの直径")]
    [SerializeField] float _radius = 0.3f;
    //[Tooltip("ホイールの重量")]
    //[SerializeField] float _mass = 10;
    [Tooltip("ばねの強さ")]
    [SerializeField] float _power;
    [Tooltip("ばねの長さ")]
    [SerializeField] float _length;
    [Tooltip("ホイールの作用点")]
    [SerializeField] Vector3 _point;
    [Tooltip("ばねの中心"), Range(0, 1)]
    [SerializeField] float _targetPosition = 0.5f;
    [Tooltip("接触するレイヤー")]
    [SerializeField] LayerMask _layerMask;


    /// <summary>車両のリジッドボディ</summary>
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
            Debug.LogWarning($"{nameof(Rigidbody)}がアタッチされていません");
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
