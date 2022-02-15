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

    /// <summary>標的のTransform</summary>
    Transform _target;
    /// <summary>サイトオブジェクトのトランスフォーム</summary>
    Transform _sight;


    // Start is called before the first frame update
    void Start()
    {
        _target = PlayerController.Instance.transform;
        _gunController = GetComponent<GunController>();
        _sight = _gunController.Sight;
    }

    // Update is called once per frame
    void Update()
    {
        if (_target)
        {
            if (Aim())
            {
                _gunController.Fire();
            }
        }
    }

    /// <summary>
    /// 照準関数
    /// </summary>
    /// <returns>照準と砲の角度の差 : true 規定値以内 : false 規定値外</returns>
    bool Aim()
    {
        _sight.LookAt(_target);

        float g = _gunController.Bullet.Gravity;
        float v = _gunController.Bullet.Speed;
        Vector3 burrel = _gunController.Barrel.transform.position;
        float h = _target.position.y - burrel.y;
        float l = Vector2.Distance(new Vector2(_target.position.x, _target.position.z), new Vector2(burrel.x, burrel.z));

        float b = -1 * (2 * v * v * l) / (g * l * l);
        float c = 1 + (2 * v * v * h) / (g * l * l);

        float d = b * b - 4 * c;

        if(d >= 0)
        {
            float t0 = Mathf.Atan((-b - Mathf.Sqrt(d)) / 2);
            float t1 = Mathf.Atan((-b + Mathf.Sqrt(d)) / 2);

            float t = Mathf.Min(t0, t1) * 180 / Mathf.PI;
            Debug.Log($"{t0 * 180 / Mathf.PI}, {t1 * 180 / Mathf.PI}, {t}");

            _sight.Rotate(new Vector3( -(t + _sight.eulerAngles.x), 0, 0));
        }
        else
        {
            return false;
        }


        float misalignment = (_gunController.Barrel.eulerAngles - _sight.eulerAngles).magnitude;
        misalignment = misalignment < 180 ? misalignment : Mathf.Abs(misalignment - 360);
        if(misalignment <= _accuracy)
        {
            return true;
        }
        return false;
    }

}
