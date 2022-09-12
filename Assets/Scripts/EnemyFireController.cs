using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 敵の火器管制コンポーネント
/// </summary>

[RequireComponent(typeof(GunController))]
public class EnemyFireController : MonoBehaviour
{
    const float radToDig = 1 / Mathf.PI * 180;

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

                if (!Physics.Raycast(_sight.position, pTarget - _sight.position, Vector3.Distance(pTarget, _sight.position), _layerMask))
                {
                    _sight.eulerAngles = Aim(pTarget);

                    //Debug.Log(misalignment);
                    if (Misalignment() <= _accuracy)
                    {
                        _gunController.Fire();
                    }
                }

                //if (Aim(pTarget).Item1)
                //{
                //    _gunController.Fire();
                //}
            }
        }
    }

    /// <summary>
    /// 照準関数
    /// </summary>
    /// <returns>照準と砲の角度の差  true 規定値以内 : false 規定値外 , </returns>
    Vector3 Aim(Vector3 target)
    {
        //_sight.LookAt(target);
        //Debug.Log(1);

        float g = _gunController.Bullet.Gravity;
        float v = _gunController.Bullet.Speed;
        Vector3 sight = _sight.transform.position;
        float h = target.y - sight.y;
        float l = Vector2.Distance(new Vector2(target.x, target.z), new Vector2(sight.x, sight.z));
        Vector3 dir = target - sight;
        Vector3 angle = new Vector3(Mathf.Atan2(dir.z, dir.y) * radToDig, Mathf.Atan2(dir.x, dir.z) * radToDig, Mathf.Atan2(dir.y, -dir.x) * radToDig);

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

            //_sight.Rotate(new Vector3(-(t + _sight.eulerAngles.x), 0, 0));
        }
        else
        {
            return new Vector3(-t, angle.y, angle.z);
        }

        return new Vector3(-t, angle.y, angle.z);
    }

    float Misalignment()
    {
        Vector2 barrel = new Vector2(_gunController.Barrel.eulerAngles.x, _gunController.Barrel.eulerAngles.y);
        Vector2 s = new Vector2(_sight.eulerAngles.x, _sight.eulerAngles.y);
        float misalignment = (barrel - s).magnitude;
        return misalignment < 180 ? misalignment : Mathf.Abs(misalignment - 360);
    }


    Vector3 Prognosis()
    {
        Vector3 target = _target.position;

        return Vector3.zero;
    }

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




//参考資料
//https://bibunsekibun.wordpress.com/2015/04/16/%E6%94%BE%E7%89%A9%E7%B7%9A%E3%81%A7%E7%9B%AE%E6%A8%99%E3%81%AB%E5%BD%93%E3%81%A6%E3%82%8B%E8%A7%92%E5%BA%A6%E3%82%92%E6%B1%82%E3%82%81%E3%82%8B/