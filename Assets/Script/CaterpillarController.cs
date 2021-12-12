using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>無限軌道の制御コンポーネント</summary>
public class CaterpillarController :MonoBehaviour
{
    [Tooltip("対のホイールの配列")]
    [SerializeField] List<Wheels> _wheels;
    [Tooltip("前進時トルク")]
    [SerializeField] float _forwardTorque;
    [Tooltip("後退時トルク")]
    [SerializeField] float _backTorque;
    [Tooltip("ブレーキのトルク")]
    [SerializeField] float _brakeTorque;
    [Tooltip("ホイールの減衰値")]
    [SerializeField] float _wheelDanpingRate;
    [Tooltip("スプリングの柔らかさ")]
    [SerializeField] float Spring;
    [Tooltip("ショックアブソーバーの強さ")]
    [SerializeField] float Damper;
    [Tooltip("サスペンションの初期位置")]
    [SerializeField] float TargetPosition;
    [Tooltip("ホイールの重量")]
    [SerializeField] float Mass;
    [Tooltip("ホイールの半径")]
    [SerializeField] float Radius;
    [Tooltip("サスペンションの最大延長距離")]
    [SerializeField] float SuspensionDistance;
    [Tooltip("ホイールの前後方向の摩擦特性")]
    [SerializeField] Friction _forwardWheelFriction;
    [Tooltip("ホイールの左右方向の摩擦特性")]
    [SerializeField] Friction _sideWheelFriction;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _wheels.Count; i++)
        {
            if (_wheels[i].RightWheel == null)
            {
                _wheels.RemoveAt(i);
                i--;
                continue;
            }
            if (_wheels[i].LeftWheel == null)
            {
                _wheels.RemoveAt(i);
                i--;
                continue;
            }
            if (_wheels[i].RightWheelMesh == null)
            {
                _wheels.RemoveAt(i);
                i--;
                continue;
            }
            if (_wheels[i].LeftWheelMesh == null)
            {
                _wheels.RemoveAt(i);
                i--;
                continue;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var w in _wheels)
        {
            Vector3 rp;
            Vector3 lp;
            Quaternion rr;
            Quaternion lr;
            w.RightWheel.GetWorldPose(out rp, out rr);
            w.LeftWheel.GetWorldPose(out lp, out lr);
            w.RightWheelMesh.transform.position = rp;
            w.RightWheelMesh.transform.rotation = rr;
            w.LeftWheelMesh.transform.position = lp;
            w.LeftWheelMesh.transform.rotation = lr;

        }
    }

    /// <summary>移動関数</summary>
    /// <param name="dir"></param>
    public void Move(Vector2 dir)
    {
        dir = dir.normalized;

    }
}

/// <summary>左右一式のホイール</summary>
[System.Serializable]
public struct Wheels
{
    [Tooltip("右ホイールコライダー")]
    public WheelCollider RightWheel;
    [Tooltip("左ホイールコライダー")]
    public WheelCollider LeftWheel;
    [Tooltip("右ホイールメッシュ")]
    public GameObject RightWheelMesh;
    [Tooltip("左ホイールメッシュ")]
    public GameObject LeftWheelMesh;
    [Tooltip("オブジェクトのローカル座標系でのホイールの中心位置")]
    public Vector3 Center;
}

/// <summary>履帯の摩擦特性</summary>
[System.Serializable]
public struct Friction
{
    [Tooltip("")]
    public float ExtremumSlip;
    [Tooltip("")]
    public float ExtremumValue;
    [Tooltip("")]
    public float AsymptoteSlip;
    [Tooltip("")]
    public float AsymptoteValue;
    [Tooltip("")]
    public float Stiffness;
}
