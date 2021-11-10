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
    [SerializeField] GameObject _barrel;
    [Tooltip("砲口")]
    [SerializeField] GameObject _muzzle;
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
        Pitch(_sight.localEulerAngles.x, transform.localEulerAngles.y);
    }

    /// <summary>砲弾の実体化から発射関数の呼び出しまでを行う</summary>
    /// <param name="root"></param>
    public void Fire(Transform root)
    {
        if (_isLoad)
        {
            var go = Instantiate(_ammos[_ammoNunber], _muzzle.transform.position, _muzzle.transform.rotation);
            go.GetComponent<BulletController>()?.Fire(root);
            StartCoroutine(Reload(_ammos[_ammoNunber].ReloadTime));
        }
    }

    /// <summary>砲身の上下の動き</summary>
    /// <param name="x"></param>
    void Pitch(float x, float y)
    {
        var dif = x - _barrel.transform.localEulerAngles.x;
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
            _barrel.transform.localEulerAngles = new Vector3(x, 0, 0);
        }
        else if(dif > _pitchSpeed)
        {
            var ro = new Vector3(_barrel.transform.localEulerAngles.x + _pitchSpeed, 0, 0);
            _barrel.transform.localEulerAngles = ro;
            _barrel.GetComponent<Rigidbody>();
        }
        else
        {
            var ro = new Vector3(_barrel.transform.localEulerAngles.x - _pitchSpeed, 0, 0);
            _barrel.transform.localEulerAngles = ro;
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
        }
        else if (dif > _yawSpeed)
        {
            var ro = new Vector3(0, transform.localEulerAngles.y + _yawSpeed, 0);
            transform.localEulerAngles = ro;
        }
        else
        {
            var ro = new Vector3(0, transform.localEulerAngles.y - _yawSpeed, 0);
            transform.localEulerAngles = ro;
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
            Debug.Log(_ammoNunber);
        }
        else
        {
            _ammoNunber = (_ammoNunber + 1) % _ammos.Length;
            Debug.Log(_ammoNunber);
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
            Debug.Log($"{transform.root.name} がリロード開始 {time}秒");
            for (_time = time; _time >= 0; _time -= Time.deltaTime)
            {
                yield return null;
            }
            Debug.Log($"{transform.root.name} がリロード完了");
            _isLoad = true;
        }
        else
        {
            _time = time;
            Debug.Log($"リロード再始 {time}秒");
        }
    }
}
