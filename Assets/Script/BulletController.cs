using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 砲弾の発射以降の操作を行うコンポーネント
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class BulletController : MonoBehaviour
{
    Rigidbody _rb;
    /// <summary>弾が放たれたかどうか</summary>
    bool _isFired;
    [Tooltip("初速")]
    [SerializeField] float _speed;
    [Tooltip("威力")]
    [SerializeField] float _power;
    /// <summary>発砲したオブジェクト</summary>
    GameObject _go;
    /// <summary>前物理フレームでの座標</summary>
    Vector3 _lastPosition;
    /// <summary>着弾した相手</summary>
    RaycastHit _hit;
    [Tooltip("弾が消滅するまでの時間")]
    [SerializeField] float _destroyTime;
    [Tooltip("リロードにかかる時間")]
    [SerializeField] float _reloadTime;
    /// <summary>リロードにかかる時間</summary>
    public float ReloadTime { get { return _reloadTime; } }
    Transform _root;
    Vector3 _firstPosition;



    private void Start()
    {
        Destroy(gameObject, _destroyTime);
    }


    private void FixedUpdate()
    {
        if (_isFired && HitCheck())   //ここにレイで着弾を観測する部分を書く
        {
            Hit(_hit.transform.root);
        }
        _lastPosition = transform.position;
    }

    /// <summary>着弾の有無、及びその対象の確認</summary>
    private bool HitCheck()
    {
        var vector = transform.position - _lastPosition;
        var distance = vector.magnitude;
        var direction = vector / distance;
        RaycastHit[] rays;
        rays = Physics.RaycastAll(_lastPosition, direction, distance);
        foreach (var r in rays)
        {
            if (r.collider.transform.root != _root)
            {
                _hit = r;
                return true;
            }
        }
        return false;
    }

    /// <summary>発砲時に呼ぶ</summary>
    public void Fire(Transform root)
    {
        _rb = GetComponent<Rigidbody>();
        _isFired = true;
        _rb.velocity = (_speed * transform.forward);
        _root = root;
        _lastPosition = transform.position;
        _firstPosition = transform.position;
    }


    /// <summary>着弾時に呼ぶ</summary>
    /// <param name="go"></param>
    void Hit(Transform t)
    {
        Debug.Log($"{t.name}に着弾　高低差{_hit.point.y - _firstPosition.y}" +
            $"　水平距離{Vector2.Distance(new Vector2(_firstPosition.x, _firstPosition.y), new Vector2(_hit.point.x, _hit.point.z))}" +
            $"　相対距離{Vector3.Distance(_firstPosition, _hit.point)}");
        t.GetComponent<CharacterBase>()?.Shot(_power);
        Destroy(gameObject);
    }

}
