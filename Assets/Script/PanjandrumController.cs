using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
/// <summary>パンジャンの制御スクリプト</summary>
public class PanjandrumController : MonoBehaviour
{
    [Tooltip("ロケットの推進力")]
    [SerializeField] float _rocketPower;
    [Tooltip("爆発の基本ダメージ")]
    [SerializeField] float _damage;
    [Tooltip("爆風の吹き飛ばす力")]
    [SerializeField] float _blastPower;
    [Tooltip("爆風の到達半径")]
    [SerializeField] float _blastRadius;
    [Tooltip("ロケットのTransform")]
    [SerializeField] Transform[] _rockets;
    [Tooltip("右ホイールのFixedJoint")]
    [SerializeField] FixedJoint _rightWheelJoint;
    [Tooltip("右ホイールのFixedJoint")]
    [SerializeField] FixedJoint _leftWheelJoint;

    Rigidbody _rb;



    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for(int i = 0; i < _rockets.Length; i++)
        {
            _rb.AddForceAtPosition(Vector3.forward * _rocketPower, _rockets[i].position);
        }
    }
}
