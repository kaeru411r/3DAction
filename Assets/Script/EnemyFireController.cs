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
    [Tooltip("射撃時の弾着予想時間の誤差許容量(秒)")]
    [SerializeField, Range(1, 3)] float _allowanceTime;

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
        Vector3 pTarget = _target.position;
        //if (_targetRb)
        //{
        //     pTarget = Prognosis();
        //}

        if (_target)
        {
            if (Aim(pTarget).Item1)
            {
                _gunController.Fire();
            }
        }
    }

    /// <summary>
    /// 照準関数
    /// </summary>
    /// <returns>照準と砲の角度の差  true 規定値以内 : false 規定値外 , </returns>
    (bool, float angle) Aim(Vector3 target)
    {
        _sight.LookAt(target);

        float g = _gunController.Bullet.Gravity;
        float v = _gunController.Bullet.Speed;
        Vector3 sight = _sight.transform.position;
        float h = target.y - sight.y;
        float l = Vector2.Distance(new Vector2(target.x, target.z), new Vector2(sight.x, sight.z));

        float b = -1 * (2 * v * v * l) / (g * l * l);
        float c = 1 + (2 * v * v * h) / (g * l * l);
        float d = b * b - 4 * c;

        float t = 0;

        if(d >= 0)
        {
            float t0 = Mathf.Atan((-b - Mathf.Sqrt(d)) / 2);
            float t1 = Mathf.Atan((-b + Mathf.Sqrt(d)) / 2);

            t = Mathf.Min(t0, t1) * 180 / Mathf.PI;
            //Debug.Log($"{t0 * 180 / Mathf.PI}, {t1 * 180 / Mathf.PI}, {t}");

            _sight.Rotate(new Vector3( -(t + _sight.eulerAngles.x), 0, 0));
        }
        else
        {
            return (false, 0);
        }

        float misalignment = (_gunController.Barrel.eulerAngles - _sight.eulerAngles).magnitude;
        misalignment = misalignment < 180 ? misalignment : Mathf.Abs(misalignment - 360);
        if(misalignment <= _accuracy)
        {
            return (true , t);
        }
        return (false, 0);
    }


    Vector3 Prognosis()
    {
        Vector3 target = _target.position;
        float angle = Aim(target).angle;
        float h0 = _sight.position.y - target.y;
        float g = _gunController.Bullet.Gravity;
        float buf0 = _gunController.Bullet.Speed * Mathf.Sin(angle);
        float h = h0 + (buf0 * buf0) / 2 * g;
        float t0 = (Mathf.Sqrt(2 * g * (h - h0)) + (Mathf.Sqrt(2 * g * h))) / g;


        target = _target.position + _targetRb.velocity * t0;
        angle = Aim(target).angle;
        h0 = _sight.position.y - target.y;
        buf0 = _gunController.Bullet.Speed * Mathf.Sin(angle);
        h = h0 + (buf0 * buf0) / 2 * g;
        float t1 = (Mathf.Sqrt(2 * g * (h - h0)) + (Mathf.Sqrt(2 * g * h))) / g;


        for (int i = 0; Mathf.Abs(t1 - t0) > _allowanceTime && i <= 100;i++)
        {
            t0 = t1;
            target = _target.position + _targetRb.velocity * t0;
            angle = Aim(target).angle;
            h0 = _sight.position.y - target.y;
            buf0 = _gunController.Bullet.Speed * Mathf.Sin(angle);
            h = h0 + (buf0 * buf0) / 2 * g;
            t1 = (Mathf.Sqrt(2 * g * (h - h0)) + (Mathf.Sqrt(2 * g * h))) / g;
        }

        if(Mathf.Abs(t1 - t0) > _allowanceTime)
        {
            Debug.LogError($"{name}で所定回数以上のループを検知 t0 = {t0} t1 = {t1}");
        }
        
        return _target.position + _targetRb.velocity * t1;
    }
}
