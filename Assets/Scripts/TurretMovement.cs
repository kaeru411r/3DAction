using UnityEngine;
using System;
public class TurretMovement : MonoBehaviour
{
    /// <summary>ミルを角度に変換する</summary>
    public const float Mil2Deg = 0.05625f;
    public const float Rad2Mil = 0.00098174770424123456789123456789f;


    [Tooltip("旋回のモード")]
    [SerializeField] Mode _turretMode = Mode.Unlimited;
    [Tooltip("俯仰のモード")]
    [SerializeField] Mode _pitchMode = Mode.Limited;
    [Tooltip("サイトのTransform")]
    [SerializeField] Transform _sight;
    [Tooltip("回転速度")]
    [SerializeField] Vector2 _speed;
    [Tooltip("砲塔のTransform")]
    [SerializeField] Transform _turret;
    [Tooltip("砲身のTransform")]
    [SerializeField] Transform _barrel;
    [Tooltip("仰角")]
    [SerializeField] float _elevationAngle = 90f;
    [Tooltip("俯角")]
    [SerializeField] float _depressionAngle = 90f;

    /// <summary>旋回のモード</summary>
    public Mode TurretMode { get => _turretMode; set => _turretMode = value; }
    /// <summary>俯仰のモード</summary>
    public Mode PitchMode { get => _pitchMode; }
    /// <summary>サイトのTransform</summary>
    public Transform Sight { get => _sight; set => _sight = value; }
    /// <summary>回転速度</summary>
    public Vector2 Speed { get => _speed; set => _speed = value; }
    /// <summary>砲塔のTransform</summary>
    public Transform TurretTransform { get => _turret; set => _turret = value; }
    /// <summary>砲身のTransform</summary>
    public Transform BarrelTransform { get => _barrel; set => _barrel = value; }
    /// <summary>仰角</summary>
    public float ElevationAngle { get => _elevationAngle; set => _elevationAngle = value; }
    /// <summary>俯角</summary>
    public float DepressionAngle { get => _depressionAngle; set => _depressionAngle = value; }
    /// <summary>照準と砲の角度の誤差</summary>
    public float Misalignment { get => Vector3.Angle(_sight.forward, _barrel.forward) * Rad2Mil; }


    private void Update()
    {
        if (_sight && _turret)
        {
            Rotate(_turret, _sight.forward, _speed.x, Time.deltaTime);
            if (_barrel)
            {
                Pitch(_barrel, _turret.up, _sight.forward, _elevationAngle, _depressionAngle, _speed.y, Time.deltaTime);
            }
        }
    }

    /// <summary>
    /// 旋回を計算する
    /// </summary>
    /// <param name="transform">動かすオブジェクトのTransform</param>
    /// <param name="forward">サイトの正面ベクトル</param>
    /// <param name="speed">旋回速度</param>
    /// <param name="deltaTime">経過時間</param>
    /// <returns>回転後のQuaternion</returns>
    void Rotate(Transform transform, in Vector3 forward, float speed, in float deltaTime)
    {
        //砲塔に対する照準器の正面のベクトル
        Vector3 dir = transform.InverseTransformDirection(forward);
        //砲塔と照準器のy軸の角度の差
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        speed *= Mil2Deg;
        //今回の処理で動かすべき角度
        angle = (speed * deltaTime < Mathf.Abs(angle)) ? (speed * deltaTime * Mathf.Sign(angle)) : angle;

        //元のrotationを必要分回転させたQuaternion
        transform.Rotate(Vector3.up, angle);
    }

    /// <summary>
    /// 砲身の動作を計算する
    /// </summary>
    /// <param name="selfTransform">動かすオブジェクトのTransform</param>
    /// <param name="baseAxis">基部の回転軸(上方向)</param>
    /// <param name="forward">サイトの正面ベクトル</param>
    /// <param name="elevation">仰角の制限</param>
    /// <param name="depression">俯角の制限</param>
    /// <param name="speed">旋回速度</param>
    /// <param name="deltaTime">経過時間</param>
    void Pitch(Transform selfTransform, in Vector3 baseAxis, in Vector3 forward, float elevation, float depression, float speed, in float deltaTime)
    {
        elevation = 90f - elevation;
        depression += 90f;

        ////基部からの角度[度]
        float sightTheta = Mathf.Clamp(Vector3.Angle(baseAxis, forward), elevation, depression);
        float selfTheta = Vector3.Angle(baseAxis, selfTransform.forward);

        speed *= Mil2Deg;

        float y = (sightTheta - selfTheta);
        float angle = (speed * deltaTime < Mathf.Abs(y)) ? (speed * deltaTime * Mathf.Sign(y)) : y;

        selfTransform.Rotate(Vector3.right, angle);
    }


    public enum Mode
    {
        Static,
        Limited,
        Unlimited,
    }
}

