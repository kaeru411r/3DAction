using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


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



    private void Start()
    {
        Destroy(gameObject, _destroyTime);
    }


    private void FixedUpdate()
    {
        if (_isFired && HitCheck())   //ここにレイで着弾を観測する部分を書く
        {
            Hit(_hit.collider.gameObject);
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
        foreach(var r in rays)
        {
            if (r.collider.gameObject != _go)
            {
                _hit = r;
                return true;
            }
        }
        return false;
    }

    /// <summary>発砲時に呼ぶ</summary>
    public void Fire(GameObject go)
    {
        _rb = GetComponent<Rigidbody>();
        _isFired = true;
        _rb.velocity = (_speed * transform.forward);
        _go = go;
        _lastPosition = transform.position;
    }


    /// <summary>着弾時に呼ぶ</summary>
    /// <param name="go"></param>
    void Hit(GameObject go)
    {
        Debug.Log($"{go.name}に着弾");
        go.GetComponent<CharacterBase>()?.Shot(_power);
        Destroy(gameObject);
    }

}
