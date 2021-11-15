using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 砲動作コンポーネント
/// 砲塔以下の操作を行う
/// </summary>
public class GunController : MonoBehaviour
{

    [Tooltip("弾薬配列")]
    [SerializeField] BulletController[] _ammos;
    [Tooltip("砲身")]
    [SerializeField] Transform _barrel;
    [Tooltip("砲口")]
    [SerializeField] Transform _muzzle;
    [Tooltip("直接照準を合わせる基準")]
    [SerializeField] Transform _sight;
    [Tooltip("砲塔の旋回速度")]
    [SerializeField] float _yawSpeed;
    [Tooltip("砲身の上下動作速度")]
    [SerializeField] float _pitchSpeed;
    /// <summary>使う弾薬の種類</summary>
    int _ammoNunber;
    /// <summary>現在装填されてるか</summary>
    bool _isLoad = true;
    /// <summary>装填完了までの時間</summary>
    float _time;


    private void Start()
    {
        StartCoroutine(Reload(_ammos[_ammoNunber].ReloadTime));
    }


    private void FixedUpdate()
    {
        Yaw(_sight.localEulerAngles.y);
        Pitch(_sight.localEulerAngles.x);
    }

    /// <summary>砲弾の実体化から発射関数の呼び出しまでを行う</summary>
    /// <param name="root"></param>
    public void Fire(Transform root)
    {
        if (_isLoad)
        {
            var go = Instantiate(_ammos[_ammoNunber], _muzzle.position, _muzzle.rotation);
            go.GetComponent<BulletController>()?.Fire(root);
            StartCoroutine(Reload(_ammos[_ammoNunber].ReloadTime));
        }
    }

    /// <summary>砲身の上下の動き</summary>
    /// <param name="x"></param>
    void Pitch(float x)
    {
        var dif = x - _barrel.localEulerAngles.x;
        if (dif < -180)
        {
            dif = dif + 360;
        }
        else if (dif > 180)
        {
            dif = dif - 360;
        }
        if (dif <= _pitchSpeed && dif >= -_pitchSpeed)
        {
            _barrel.localEulerAngles = new Vector3(x, 0, 0);
            _barrel.Rotate(Vector3.zero);
        }
        else if(dif > _pitchSpeed)
        {
            _barrel.Rotate(_pitchSpeed, 0, 0);
        }
        else
        {
            _barrel.Rotate(-_pitchSpeed, 0, 0);
        }
    }

    /// <summary>砲塔の旋回</summary>
    /// <param name="y"></param>
    void Yaw(float y)
    {
        var dif = y - transform.localEulerAngles.y;
        if (dif < -180)
        {
            dif = dif + 360;
        }
        else if (dif > 180)
        {
            dif = dif - 360;
        }
        if (dif <= _yawSpeed && dif >= -_yawSpeed)
        {
            transform.localEulerAngles = new Vector3(0, y, 0);
            transform.Rotate(Vector3.zero);
        }
        else if (dif > _yawSpeed)
        {
            transform.Rotate(0, _yawSpeed, 0);
        }
        else
        {
            transform.Rotate(0, -_yawSpeed, 0);
        }
    }

    /// <summary>砲弾の切り替えを行う</summary>
    /// <param name="f"></param>
    public void Choice(float f)
    {
        if (f < 0)
        {
            _ammoNunber = _ammoNunber - 1;
            if (_ammoNunber < 0)
                _ammoNunber = _ammos.Length - 1;
        }
        else
        {
            _ammoNunber = (_ammoNunber + 1) % _ammos.Length;
        }
        StartCoroutine(Reload(_ammos[_ammoNunber].ReloadTime));
    }


    /// <summary>砲弾の装填を行う
    /// 最後に呼び出されてからtime秒後に装填完了する</summary>
    IEnumerator Reload(float time)
    {
        if (_isLoad)
        {
            _isLoad = false;
            for (_time = time; _time >= 0; _time -= Time.deltaTime)
            {
                yield return null;
            }
            _isLoad = true;
        }
        else
        {
            _time = time;
        }
    }
}
