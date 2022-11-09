using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 敵の火器管制コンポーネント
/// </summary>

public class EnemyFireController : MonoBehaviour
{
    const float radToDig = 1 / Mathf.PI * 180;

    /// <summary>照準の時間差計算の回数</summary>
    static float calculationNumber = 5;

    /// <summary>照準の時間差計算の回数</summary>
    public static float CalculationNumber { get => calculationNumber; set => calculationNumber = value; }


    [Tooltip("このオブジェクトのGunController")]
    [SerializeField] Turret _turret;
    [Tooltip("射撃時の許容誤差(角度)")]
    [SerializeField] float _accuracy;
    //[Tooltip("射撃時の弾着予想時間の誤差許容量(秒)")]
    //[SerializeField, Range(1, 3)] float _allowanceTime;
    [Tooltip("弾道")]
    [SerializeField] AimMode _aimMode = AimMode.PointBlank;
    [Tooltip("索敵範囲")]
    [SerializeField] float _range = 100;
    [Tooltip("Defaltレイヤーを選択")]
    [SerializeField] LayerMask _layerMask;
    [SerializeField] Transform _target;

    /// <summary>標的のTransform</summary>
    //Transform _target;
    /// <summary>標的のRigidbody</summary>
    Rigidbody _targetRb;
    /// <summary>サイトオブジェクトのトランスフォーム</summary>
    Transform _sight;
    /// <summary>サイトオブジェクトのトランスフォーム</summary>
    Transform _muzzle;
    GameObjectCollector _collector;



    // Start is called before the first frame update
    void Start()
    {
        //_target = new Target(PlayerController.Instance.transform);
        _sight = _turret.Sight;
        _muzzle = _turret.Muzzle;
        if (!_muzzle)
        {
            _sight = _turret.Barrel;
            if (!_muzzle)
            {
                _sight = _turret.Sight;
            }
        }
        _collector = new GameObjectCollector(name);
        Time();
    }

    void Time()
    {
        if (_targetRb && _targetRb.velocity.magnitude != 0 && _target)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = -500; i < 500; i++)
                {
                    float? time1 = FringTime(_target.position + _targetRb.velocity * i);
                    if(time1 != null)
                    {
                        sb.AppendLine(time1.Value.ToString());
                    }
                }
            Debug.Log(sb.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!(_target && _sight))
        {
            return;
        }
        if (Vector3.Distance(transform.position, _target.position) <= _range)
        {
            Vector3 pTarget = _target.position;

            if (!Physics.Raycast(_sight.position, pTarget - _sight.position,out RaycastHit hit, Vector3.Distance(pTarget, _sight.position), _layerMask))
            {
                Vector3? target2 = Prognosis(pTarget);
                if (target2 == null)
                {
                    return;
                }
                Vector3? angle = Aim(target2.Value);
                //Vector3? angle = Aim(pTarget);
                if (angle != null)
                {
                    _sight.eulerAngles = angle.Value;
                    if (Misalignment() <= _accuracy)
                    {
                        List<BaseBullet> bullets = _turret.Fire();
                        foreach (BaseBullet bullet in bullets)
                        {
                            _collector.Collection(bullet);
                        }
                        if (bullets != null)
                        {
                            float ft = FringTime(target2.Value).Value;
                            System.DateTime now = System.DateTime.Now;
                            float time = (now.Hour * 3600) + (now.Minute * 60) + now.Second + now.Millisecond / 1000f + ft;
                            Debug.Log($"発射 着弾まで後{ft.ToString("F3")}秒 " +
                                $"着弾予想時刻{((int)((time % 86400) / 3600f)).ToString("D2")}:{((int)((time % 3600) / 60)).ToString("D2")}:{((int)(time % 60)).ToString("D2")} " +
                                $"着弾予想座標{target2}");
                        }
                    }
                }
            }
            else
            {
                Debug.Log(hit.collider.gameObject);
            }

            //if (Aim(pTarget).Item1)
            //{
            //    _gunController.Fire();
            //}
        }
    }

#if UNITY_EDITOR

    private void OnValidate()
    {
        if (_accuracy < 0)
        {
            _accuracy = 0;
        }
        else if (_accuracy > 180)
        {
            _accuracy = 180;
        }

        if (_range < 0)
        {
            _range = 0;
        }
        if (_target)
        {
            _targetRb = _target.gameObject.GetComponent<Rigidbody>();
        }
    }

#endif


    /// <summary>
    /// 照準関数
    /// </summary>
    /// <returns>照準の角度 nullなら射程外</returns>
    Vector3? Aim(Vector3 target)
    {
        //_sight.LookAt(target);
        //Debug.Log(1);
        if (!_muzzle)
        {
            return null;
        }
        Vector3 dir = target - _muzzle.position;
        Vector3 angle = new Vector3(Mathf.Atan2(dir.z, dir.y) * radToDig, Mathf.Atan2(dir.x, dir.z) * radToDig, Mathf.Atan2(dir.y, -dir.x) * radToDig);
        float? t = FiringElevation(target);
        if (t != null)
        {
            return new Vector3(-t.Value, angle.y, angle.z);
        }
        else
        {
            return null;
        }
    }


    /// <summary>
    /// 射角算出
    /// </summary>
    /// <param name="target"></param>
    /// <returns>射角[度] nullなら射程外</returns>
    float? FiringElevation(Vector3 target)
    {
        float g = _turret.Gravity;
        float v = _turret.Speed;
        float h = target.y - _muzzle.position.y;
        float l = Vector2.Distance(new Vector2(target.x, target.z), new Vector2(_muzzle.position.x, _muzzle.position.z));

        //tan(theta)の二次関数 a * tan(theta) ^ 2 + b * tan(theta) + cの係数 (aは1なので省略)
        float b = -1 * (2 * v * v * l) / (g * l * l);
        float c = 1 + (2 * v * v * h) / (g * l * l);

        //二次関数の解が存在するかを確かめる判別式
        float d = b * b - 4 * c;


        if (d >= 0)
        {
            float theta;
            float t0 = Mathf.Atan((-b - Mathf.Sqrt(d)) / 2);
            float t1 = Mathf.Atan((-b + Mathf.Sqrt(d)) / 2);

            if (_aimMode == AimMode.PointBlank)
            {
                theta = Mathf.Min(t0, t1) * 180 / Mathf.PI;
            }
            else
            {
                theta = Mathf.Max(t0, t1) * 180 / Mathf.PI;
            }
            //Debug.Log($"{t0 * 180 / Mathf.PI}, {t1 * 180 / Mathf.PI}, {t}");

            //_sight.Rotate(new Vector3(-(t + _sight.eulerAngles.x), 0, 0));
            return theta;
        }
        else
        {
            return null;
        }
    }

    float? FringTime(Vector3 target)
    {
        float? theta = FiringElevation(target);
        if (theta == null)
        {
            return null;
        }
        float v = _turret.Speed;
        Vector2 sight = new Vector2(_muzzle.position.x, _muzzle.position.z);
        Vector2 targetXZ = new Vector2(target.x, target.z);
        float x = Vector2.Distance(sight, targetXZ);
        return x / (v * Mathf.Cos(theta.Value / 180 * Mathf.PI));
    }

    /// <summary>
    /// 照準と砲の角度の誤差を求める
    /// </summary>
    /// <returns>照準と法の角度の誤差[度]</returns>
    float Misalignment()
    {
        Vector3 barrel = _turret.Barrel.forward;
        Vector3 sight = _sight.forward;
        float misalignment = Mathf.Acos((Vector3.Dot(barrel, sight) / (barrel.magnitude * sight.magnitude))) * radToDig;
        return misalignment <= 180 ? misalignment : Mathf.Abs(misalignment - 360);
    }

    /// <summary>
    /// 標的の移動に合わせ理想の着弾位置を算出する
    /// </summary>
    /// <returns>座標</returns>
    Vector3? Prognosis(Vector3 target)
    {
        if (!_targetRb) { return target; }
        if (_targetRb.velocity.magnitude == 0) { return target; }
        //Vector3 target2 = target;
        //for (int i = 0; i < calculationNumber; i++)
        //{
        //    float? time = FringTime(target2);
        //    if (time != null)
        //    {
        //        float? time2 = FringTime(target + _targetRb.velocity * time.Value);
        //        if (time2 != null)
        //        {
        //            float? time3 = time + (time2 - time2) * 2;
        //            float? time4 = FringTime(target + _targetRb.velocity * time3.Value);
        //            if (time4 != null)
        //            {
        //                float hi = (time3.Value - time4.Value) / (time2.Value - time.Value);
        //                target2 = target + _targetRb.velocity * (time2.Value + (time4.Value - time2.Value) * hi);
        //            }
        //            else { return target; }
        //        }
        //        else { return target; }
        //    }
        //    else { return target; }
        //}
        //return target2;
        float? time = FringTime(target);
        if (time == null) { return null; }
        Vector3 target2 = target + _targetRb.velocity * time.Value;
        for (int i = 0; i < calculationNumber; i++)
        {
            time = FringTime(target2);
            if (time == null)
            {
                return null;
            }
            target2 = target + _targetRb.velocity * time.Value;
        }
        return target2;
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