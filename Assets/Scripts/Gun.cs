using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;


/// <summary>
/// 砲動作コンポーネント
/// 砲塔以下の操作を行う
/// </summary>
public class Gun : MonoBehaviour
{
    [Tooltip("弾薬配列")]
    [SerializeField] List<Bullet> _ammos;
    [Tooltip("砲身")]
    [SerializeField] Transform _barrel;
    [Tooltip("砲口")]
    [SerializeField] Transform _muzzle;

    /// <summary>使う弾薬の種類</summary>
    int _ammoNunber;
    /// <summary>現在装填されてるか</summary>
    bool _isLoad = true;
    /// <summary>装填完了までの時間</summary>
    float _time;
    /// <summary>戦車のリジッドボディ</summary>
    Rigidbody _rb;
    /// <summary>必要なTransformがアサインされてなかったときのダミー</summary>
    Transform _dummy;

    /// <summary>装填が済んでいるか</summary>
    public bool IsLoad { get => _isLoad; }
    /// <summary>バレル</summary>
    public Transform Barrel { get { return _barrel; } }
    /// <summary>マズル</summary>
    public Transform Muzzle { get { return _muzzle; } }



    /// <summary>現在選択中の弾薬</summary>
    public Bullet Bullet
    {
        get
        {
            if (_ammos[_ammoNunber] != null)
            {
                return _ammos[_ammoNunber];
            }
            else
            {
                Debug.LogError($"{name}の{nameof(_ammos)}には{_ammoNunber}はありません");
                return null;
            }
        }
    }

    private void Start()
    {
        if (_ammos.Count == 0)
        {
            Debug.LogError($"{name}はAmmosが選択されていません");
            _isLoad = false;
        }
        else if (_ammos.Contains(null))
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0, n = 0; i < _ammos.Count; i++, n++)
            {
                if (!_ammos[i])
                {
                    sb.Append($"\nElement{n}");
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
                sb.Insert(0, $"{name}の{this}は{nameof(_ammos)}の以下の項目にアサインがされていません");
                Debug.LogWarning(sb);
            }
        }
        if (!_barrel)
        {
            Debug.LogError($"{name}は{nameof(_barrel)}がアサインされていません");
            _barrel = PreDummy();
        }
        if (!_muzzle)
        {
            Debug.LogError($"{name}は{nameof(_muzzle)}がアサインされていません");
            _muzzle = PreDummy();
        }
        _rb = GetComponent<Rigidbody>();
    }

    /// <summary>砲弾の実体化から発射関数の呼び出しまでを行う</summary>
    /// <param name="root"></param>
    /// <returns>発砲したか否か</returns>
    public bool Fire()
    {
        if (_isLoad)
        {
            Vector3 dir = new Vector3(_muzzle.eulerAngles.x + 90, _muzzle.eulerAngles.y, _muzzle.eulerAngles.z);
            var go = Instantiate(Bullet, _muzzle.position, _muzzle.rotation);
            go.GetComponent<Bullet>()?.Fire(transform);

            if (_rb)
            {
                float mass = go.Mass;
                _rb.AddForceAtPosition(-_muzzle.forward * mass * Bullet.Speed, _muzzle.position, ForceMode.Impulse);
            }

            StartCoroutine(Reload(Bullet.ReloadTime));
            return true;
        }
        return false;
    }

    /// <summary>砲弾の切り替えを行う</summary>
    /// <param name="f"></param>
    public bool Change(float f)
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

            return true;
        }

        return false;
    }

    /// <summary>砲弾の選択を行う</summary>
    /// <param name="n"></param>
    public bool Choice(int n)
    {
        if (_ammos.Count != 0)
        {
            if (n <= _ammos.Count && n - 1 != _ammoNunber)
            {
                _ammoNunber = n - 1;
                StartCoroutine(Reload(_ammos[_ammoNunber].ReloadTime));
            }

            return true;
        }

        return false;
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
