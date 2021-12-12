using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>無限軌道の制御コンポーネント</summary>
public class CaterpillarController : MonoBehaviour
{
    [Tooltip("対のホイールの配列")]
    [SerializeField] List<WheelColliders> _wheelColliders;
    [Tooltip("ホイールのMesh")]
    [SerializeField] List<WheelMeshs> _wheelMeshs;
    [Tooltip("前進時トルク")]
    [SerializeField] float _forwardTorque;
    [Tooltip("後退時トルク")]
    [SerializeField] float _backTorque;
    [Tooltip("ブレーキのトルク")]
    [SerializeField] float _brakeTorque;
    [Tooltip("ホイールの減衰値")]
    [SerializeField] float _wheelDanpingRate;
    [Tooltip("スプリングの柔らかさ")]
    [SerializeField] float _spring;
    [Tooltip("ショックアブソーバーの強さ")]
    [SerializeField] float _damper;
    [Tooltip("サスペンションの初期位置")]
    [SerializeField] float _targetPosition;
    [Tooltip("ホイールの重量")]
    [SerializeField] float _mass;
    [Tooltip("ホイールの半径")]
    [SerializeField] float _radius;
    [Tooltip("サスペンションの最大延長距離")]
    [SerializeField] float _suspensionDistance;
    [Tooltip("ホイールの前後方向の摩擦特性")]
    [SerializeField] Friction _forwardWheelFriction;
    [Tooltip("ホイールの左右方向の摩擦特性")]
    [SerializeField] Friction _sideWheelFriction;

    // Start is called before the first frame update
    void Start()
    {
        NullCheck();
        SetUp();
    }

    /// <summary>_wheelsの各要素に必要なものがそろっているか</summary>
    void NullCheck()
    {
        StringBuilder sb = new StringBuilder();
        bool b0 = false;
        int l = 0;
        for (int i = 0; i < _wheelColliders.Count; i++)
        {
            bool b1 = false;
            if (_wheelColliders[i].RightWheel == null)
            {
                sb.Append($"\nElement{l}");
                sb.Append($"\n  {nameof(WheelColliders.RightWheel)}");
                b1 = true;
            }
            if (_wheelColliders[i].LeftWheel == null)
            {
                if (!b1)
                {
                    sb.Append($"\nElement{l}");
                }
                sb.Append($"\n  {nameof(WheelColliders.LeftWheel)}");
                b1 = true;
            }
            if (_wheelMeshs[i].RightWheelMesh == null)
            {
                if (!b1)
                {
                    sb.Append($"\nElement{l}");
                }
                sb.Append($"\n  {nameof(WheelMeshs.RightWheelMesh)}");
                b1 = true;
            }
            if (_wheelMeshs[i].LeftWheelMesh == null)
            {
                if (!b1)
                {
                    sb.Append($"\nElement{l}");
                }
                sb.Append($"\n  {nameof(WheelMeshs.LeftWheelMesh)}");
                b1 = true;
            }
            if (b1)
            {
                _wheelColliders.RemoveAt(i);
                i--;
                b0 = true;
            }
            l++;
        }
        if (b0)
        {
            sb.Insert(0, $"{transform.root.name}の{nameof(_wheelColliders)}の以下の項目にアサインがされていません");
            Debug.LogWarning(sb);
        }
    }

    /// <summary>各ホイールのパラメータの設定</summary>
    void SetUp()
    {
        JointSpring j = new JointSpring();
        WheelFrictionCurve f = new WheelFrictionCurve();
        foreach (var w in _wheelColliders)
        {
            var r = w.RightWheel;
            var l = w.LeftWheel;
            r.wheelDampingRate = _wheelDanpingRate;
            l.wheelDampingRate = _wheelDanpingRate;
            j.spring = _spring;
            j.damper = _damper;
            j.targetPosition = _targetPosition;
            r.suspensionSpring = j;
            l.suspensionSpring = j;
            r.mass = _mass;
            l.mass = _mass;
            r.radius = _radius;
            l.radius = _radius;
            r.suspensionDistance = _suspensionDistance;
            l.suspensionDistance = _suspensionDistance;
            f.extremumSlip = _forwardWheelFriction.ExtremumSlip;
            f.extremumValue = _forwardWheelFriction.ExtremumValue;
            f.asymptoteSlip = _forwardWheelFriction.AsymptoteSlip;
            f.asymptoteValue = _forwardWheelFriction.AsymptoteValue;
            f.stiffness = _forwardWheelFriction.Stiffness;
            r.forwardFriction = f;
            l.forwardFriction = f;
            f.extremumSlip = _sideWheelFriction.ExtremumSlip;
            f.extremumValue = _sideWheelFriction.ExtremumValue;
            f.asymptoteSlip = _sideWheelFriction.AsymptoteSlip;
            f.asymptoteValue = _sideWheelFriction.AsymptoteValue;
            f.stiffness = _sideWheelFriction.Stiffness;
            r.sidewaysFriction = f;
            l.sidewaysFriction = f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rp;
        Vector3 lp;
        Quaternion rr;
        Quaternion lr;
        WheelMeshs w;
        for(int i = 0; i < _wheelMeshs.Count; i++)
        {
            w = _wheelMeshs[i];
            if (_wheelColliders.Count <= w.Index)
            {
                Debug.LogError($"{transform.root.name}の{nameof(_wheelColliders)}には{w.Index}は無いため、" +
                    $"{nameof(_wheelMeshs)}[{i}]は" +
                    $"{nameof(_wheelColliders)}[{_wheelColliders.Count - 1}]と同期されます");
                w.Index = _wheelColliders.Count - 1;
                _wheelMeshs[i] = w;
            }
            _wheelColliders[w.Index].RightWheel.GetWorldPose(out rp, out rr);
            _wheelColliders[w.Index].LeftWheel.GetWorldPose(out lp, out lr);
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

        foreach (var w in _wheelColliders)
        {
            if (dir.y == 0)
            {
                w.RightWheel.brakeTorque = _brakeTorque;
                w.LeftWheel.brakeTorque = _brakeTorque;
                w.RightWheel.motorTorque = 0;
                w.LeftWheel.motorTorque = 0;
            }
            else if (dir.y > 0)
            {
                w.RightWheel.brakeTorque = 0;
                w.LeftWheel.brakeTorque = 0;
                w.RightWheel.motorTorque = dir.y * _forwardTorque;
                w.LeftWheel.motorTorque = dir.y * _forwardTorque;
            }
            else if (dir.y < 0)
            {
                w.RightWheel.motorTorque = dir.y * _backTorque;
                w.LeftWheel.motorTorque = dir.y * _backTorque;
                w.RightWheel.brakeTorque = 0;
                w.LeftWheel.brakeTorque = 0;
            }
        }

    }
}

/// <summary>左右一式のホイールコライダー</summary>
[System.Serializable]
public struct WheelColliders
{
    [Tooltip("右ホイールコライダー")]
    public WheelCollider RightWheel;
    [Tooltip("左ホイールコライダー")]
    public WheelCollider LeftWheel;
    [Tooltip("オブジェクトのローカル座標系でのホイールの中心位置")]
    public Vector3 Center;
}

/// <summary>左右一式のホイールの見た目</summary>
[System.Serializable]
public struct WheelMeshs
{
    [Tooltip("何番のホイールと同期させるか")]
    public int Index;
    [Tooltip("右ホイールメッシュ")]
    public GameObject RightWheelMesh;
    [Tooltip("左ホイールメッシュ")]
    public GameObject LeftWheelMesh;
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
