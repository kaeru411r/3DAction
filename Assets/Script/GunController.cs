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
    /// <summary>使う弾薬の種類</summary>
    int _ammoNunber;
    bool _isLoad = true;
    float _time;

    private void Start()
    {
        StartCoroutine(Reload(_ammos[_ammoNunber].ReloadTime));
    }

    public void Fire(Transform transform)
    {
        if (_isLoad)
        {
            Debug.Log($"name = {name}{transform}");
            foreach (var tr in transform)
            {
                Debug.Log($"trnsform = {tr}");
            }
            var go = Instantiate(_ammos[_ammoNunber], _muzzle.transform.position, _muzzle.transform.rotation);
            go.GetComponent<BulletController>()?.Fire(transform);
            StartCoroutine(Reload(_ammos[_ammoNunber].ReloadTime));
        }
    }

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

    IEnumerator Reload(float time)
    {
        if (_isLoad)
        {
            _isLoad = false;
            Debug.Log($"リロード開始 {time}秒");
            for (_time = time; _time >= 0; _time -= Time.deltaTime)
            {
                yield return null;
            }
            Debug.Log("リロード完了");
            _isLoad = true;
        }
        else
        {
            _time = time;
            Debug.Log($"リロード再始 {time}秒");
        }
    }
}
