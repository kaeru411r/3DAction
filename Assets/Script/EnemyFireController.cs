using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の火器管制コンポーネント
/// </summary>

[RequireComponent(typeof(GunController))]
public class EnemyFireController : MonoBehaviour
{
    /// <summary>このオブジェクトのGunController</summary>
    GunController _gunController;

    [Tooltip("射撃時の精度")]
    [SerializeField] float _accuracy;
    //[Tooltip("射撃時の弾着予想時間の誤差許容量(秒)")]
    //[SerializeField, Range(1, 3)] float _allowanceTime;
    [Tooltip("弾道")]
    [SerializeField] AimMode _aimMode = AimMode.PointBlank;
    [Tooltip("索敵範囲")]
    [SerializeField] float _range = 100;
    [Tooltip("Defaltレイヤーを選択")]
    [SerializeField] LayerMask _layerMask;

    /// <summary>標的のTransform</summary>
    Transform _target;
    /// <summary>標的のRigidbody</summary>
    Rigidbody _targetRb;
    /// <summary>サイトオブジェクトのトランスフォーム</summary>
    Transform _sight;


    // Start is called before the first frame update
    void Start()
    {
        _target = PlayerController.Instance.transform;
        _gunController = GetComponent<GunController>();
        _sight = _gunController.Sight;
        if (_target)
        {
            _targetRb = _target.GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_target)
        {
            if (Vector3.Distance(transform.position, _target.position) <= _range)
            {
                Vector3 pTarget = _target.position;
                //if (_targetRb)
                //{
                //     pTarget = Prognosis();
                //}

                if (Aim(pTarget).Item1)
                {
                    _gunController.Fire();
                }
            }
        }
    }

    /// <summary>
    /// 照準関数
    /// </summary>
    /// <returns>照準と砲の角度の差  true 規定値以内 : false 規定値外 , </returns>
    (bool, float angle) Aim(Vector3 target)
    {
        if (!Physics.Raycast(_sight.position, target - _sight.position, Vector3.Distance(target, _sight.position), _layerMask)){
            _sight.LookAt(target);
            //Debug.Log(1);

            float g = _gunController.Bullet.Gravity;
            float v = _gunController.Bullet.Speed;
            Vector3 sight = _sight.transform.position;
            float h = target.y - sight.y;
            float l = Vector2.Distance(new Vector2(target.x, target.z), new Vector2(sight.x, sight.z));

            //tan(theta)の二次関数 a * tan(theta) ^ 2 + b * tan(theta) + cの係数 (aは1なので省略)
            float b = -1 * (2 * v * v * l) / (g * l * l);
            float c = 1 + (2 * v * v * h) / (g * l * l);

            //二次関数の解が存在するかを確かめる判別式
            float d = b * b - 4 * c;

            float t = 0;

            if (d >= 0)
            {
                float t0 = Mathf.Atan((-b - Mathf.Sqrt(d)) / 2);
                float t1 = Mathf.Atan((-b + Mathf.Sqrt(d)) / 2);

                if (_aimMode == AimMode.PointBlank)
                {
                    t = Mathf.Min(t0, t1) * 180 / Mathf.PI;
                }
                else
                {
                    t = Mathf.Max(t0, t1) * 180 / Mathf.PI;
                }
                //Debug.Log($"{t0 * 180 / Mathf.PI}, {t1 * 180 / Mathf.PI}, {t}");

                _sight.Rotate(new Vector3(-(t + _sight.eulerAngles.x), 0, 0));
            }
            else
            {
                return (false, 0);
            }

            Vector2 barrel = new Vector2(_gunController.Barrel.eulerAngles.x, _gunController.Barrel.eulerAngles.y);
            Vector2 s = new Vector2(_sight.eulerAngles.x, _sight.eulerAngles.y);
            float misalignment = (barrel - s).magnitude;
            misalignment = misalignment < 180 ? misalignment : Mathf.Abs(misalignment - 360);
            //Debug.Log(misalignment);
            if (misalignment <= _accuracy)
            {
                return (true, t);
            }
        }
        return (false, 0);
    }


    //Vector3 Prognosis()
    //{
    //    Vector3 target = _target.position;
    //    float angle = Aim(target).angle;
    //    float h0 = _sight.position.y - target.y;
    //    float g = _gunController.Bullet.Gravity;
    //    float buf0 = _gunController.Bullet.Speed * Mathf.Sin(angle);
    //    float h = h0 + (buf0 * buf0) / 2 * g;
    //    float t0 = (Mathf.Sqrt(2 * g * (h - h0)) + (Mathf.Sqrt(2 * g * h))) / g;


    //    target = _target.position + _targetRb.velocity * t0;
    //    angle = Aim(target).angle;
    //    h0 = _sight.position.y - target.y;
    //    buf0 = _gunController.Bullet.Speed * Mathf.Sin(angle);
    //    h = h0 + (buf0 * buf0) / 2 * g;
    //    float t1 = (Mathf.Sqrt(2 * g * (h - h0)) + (Mathf.Sqrt(2 * g * h))) / g;


    //    for (int i = 0; Mathf.Abs(t1 - t0) > _allowanceTime && i <= 100;i++)
    //    {
    //        t0 = t1;
    //        target = _target.position + _targetRb.velocity * t0;
    //        angle = Aim(target).angle;
    //        h0 = _sight.position.y - target.y;
    //        buf0 = _gunController.Bullet.Speed * Mathf.Sin(angle);
    //        h = h0 + (buf0 * buf0) / 2 * g;
    //        t1 = (Mathf.Sqrt(2 * g * (h - h0)) + (Mathf.Sqrt(2 * g * h))) / g;
    //    }

    //    if(Mathf.Abs(t1 - t0) > _allowanceTime)
    //    {
    //        Debug.LogError($"{name}で所定回数以上のループを検知 t0 = {t0} t1 = {t1}");
    //    }

    //    return _target.position + _targetRb.velocity * t1;
    //}

    /// <summary>
    /// 弾道
    /// </summary>
    enum AimMode
    {
        /// <summary>直射</summary>
        PointBlank,
        /// <summary>曲射</summary>
        HighAngle,
    }
}
