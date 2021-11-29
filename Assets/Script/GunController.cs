using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


/// <summary>
/// 砲動作コンポーネント
/// 砲塔以下の操作を行う
/// </summary>
public class GunController : MonoBehaviour
{

    [Tooltip("弾薬配列")]
    [SerializeField] List<BulletController> _ammos;
    [Tooltip("砲塔")]
    [SerializeField] Transform _turret;
    [Tooltip("砲身")]
    [SerializeField] Transform _barrel;
    [Tooltip("砲口")]
    [SerializeField] Transform _muzzle;
    [Tooltip("直接照準を合わせる基準")]
    [SerializeField] Transform _sight;
    [Tooltip("砲の動作スピード")]
    [SerializeField] Vector2 _gunMoveSpeed;
    [Tooltip("仰角")]
    [SerializeField] float _elevationAngle = 90;
    [Tooltip("俯角")]
    [SerializeField] float _depressionAngle = 90;
    /// <summary>sightが狙う先</summary>
    Transform _target;
    /// <summary>使う弾薬の種類</summary>
    int _ammoNunber;
    /// <summary>現在装填されてるか</summary>
    bool _isLoad = true;
    /// <summary>装填完了までの時間</summary>
    float _time;
    /// <summary>360度</summary>
    const int AllAround = 360;
    /// <summary>必要なTransformがアサインされてなかったときのダミー</summary>
    Transform _dummy;

    public Transform Sight { get { return _sight; } set { _sight = value; } }

    public Vector3 Barrel { get { return _barrel.eulerAngles; } }

    public Vector3 Turret { get { return _turret.eulerAngles; } }

    public Vector2 GunMoveSpeed { get { return _gunMoveSpeed; } }
    int a = 0;

    private void Start()
    {
        if (_ammos.Count == 0)
        {
            Debug.LogError($"{name}はAmmosが選択されていません");
            _isLoad = false;
        }
        else if (_ammos.Contains(null))
        {
            for (int i = 0; i < _ammos.Count; i++)
            {
                if (!_ammos[i])
                {
                    _ammos.RemoveAt(i);
                    i--;
                }
            }
            if (_ammos.Count == 0)
            {
                Debug.LogError($"{name}はAmmosが選択されていません");
                _isLoad = false;
            }
            else
            {
                Debug.LogWarning($"{name}はAmmosに未選択があります");
            }
        }
        if (!_turret)
        {
            Debug.LogError($"{name}はTurretが選択されていません");
            _turret = PreDummy();
        }
        if (!_barrel)
        {
            Debug.LogError($"{name}はBurrelが選択されていません");
            _barrel = PreDummy();
        }
        if (!_muzzle)
        {
            Debug.LogError($"{name}はMuzzleが選択されていません");
            _muzzle = PreDummy();
        }
        if (!_sight)
        {
            Debug.LogError($"{name}はSightが選択されていません");
            _sight = PreDummy();
        }
    }

    private void Update()
    {
        Debug.Log($"gun{a}");
        a++;
        float x = _sight.eulerAngles.x;
        if (x > AllAround / 2)
        {
            x -= AllAround;
        }
        if (x < -_elevationAngle)
        {
            //Debug.Log($"#1 {_sight.eulerAngles.x} {-_elevationAngle} {_sight.eulerAngles.x < -_elevationAngle}");
            _sight.eulerAngles = new Vector3(-_elevationAngle, _sight.eulerAngles.y, _sight.eulerAngles.z);
        }
        else if (x > _depressionAngle)
        {
            //Debug.Log($"#2 {_sight.eulerAngles.x} {-_depressionAngle} {_sight.eulerAngles.x > _depressionAngle}");
            _sight.eulerAngles = new Vector3(_depressionAngle, _sight.eulerAngles.y, _sight.eulerAngles.z);
        }
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
        if (dif < -AllAround / 2)
        {
            dif = dif + AllAround;
        }
        else if (dif > AllAround / 2)
        {
            dif = dif - AllAround;
        }
        if (dif <= _gunMoveSpeed.y && dif >= -_gunMoveSpeed.y)
        {
            _barrel.localEulerAngles = new Vector3(x, 0, 0);
        }
        else if (dif > _gunMoveSpeed.y)
        {
            _barrel.Rotate(_gunMoveSpeed.y, 0, 0);
        }
        else
        {
            _barrel.Rotate(-_gunMoveSpeed.y, 0, 0);
        }
    }

    /// <summary>砲塔の旋回</summary>
    /// <param name="y"></param>
    void Yaw(float y)
    {
        var dif = y - _turret.transform.localEulerAngles.y;
        if (dif < -AllAround / 2)
        {
            dif = dif + AllAround;
        }
        else if (dif > AllAround / 2)
        {
            dif = dif - AllAround;
        }
        if (dif <= _gunMoveSpeed.x && dif >= -_gunMoveSpeed.x)
        {
            _turret.transform.localEulerAngles = new Vector3(0, y, 0);
        }
        else if (dif > _gunMoveSpeed.x)
        {
            _turret.transform.Rotate(0, _gunMoveSpeed.x, 0);
        }
        else
        {
            _turret.transform.Rotate(0, -_gunMoveSpeed.x, 0);
        }
    }

    /// <summary>砲弾の切り替えを行う</summary>
    /// <param name="f"></param>
    public void Change(float f)
    {
        if (_ammos.Count != 0)
        {
            if (f < 0)
            {
                _ammoNunber = _ammoNunber - 1;
                if (_ammoNunber < 0)
                    _ammoNunber = _ammos.Count - 1;
            }
            else
            {
                _ammoNunber = (_ammoNunber + 1) % _ammos.Count;
            }
            StartCoroutine(Reload(_ammos[_ammoNunber].ReloadTime));
        }
    }

    public void Choice(int n)
    {
        if (_ammos.Count != 0)
        {
            if (n <= _ammos.Count && n - 1 != _ammoNunber)
            {
                _ammoNunber = n - 1;
                StartCoroutine(Reload(_ammos[_ammoNunber].ReloadTime));
            }
        }
    }


    /// <summary>砲弾の装填を行う
    /// 最後に呼び出されてからtime秒後に装填完了する</summary>
    IEnumerator Reload(float time)
    {
        if (_isLoad)
        {
            Debug.Log($"{transform.root.name}が{_ammos[_ammoNunber].name}装填完了まで{time}");
            _isLoad = false;
            for (_time = time; _time >= 0; _time -= Time.deltaTime)
            {
                yield return null;
            }
            Debug.Log($"{transform.root.name}がリロード完了");
            _isLoad = true;
        }
        else
        {
            _time = time;
            Debug.Log($"{transform.root.name}が{_ammos[_ammoNunber].name}装填完了まで{_time}");
        }
    }

    Transform PreDummy()
    {
        if (_dummy)
        {
            return _dummy;
        }
        else
        {
            _dummy = new GameObject().transform;
            return _dummy;
        }
    }
}
