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
    /// <summary>着弾時に呼ぶ関数</summary>
    event Action OnHit;
    /// <summary>前フレームでの座標</summary>
    Vector3 _lastPosition;
    RaycastHit _hit;



    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _lastPosition = transform.position;
        Fire(gameObject);
    }


    private void FixedUpdate()
    {
        if (_isFired && HitCheck() && _hit.collider.gameObject != _go)   //ここにレイで着弾を観測する部分を書く
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
        Ray ray = new Ray(_lastPosition, direction);
        if(Physics.Raycast(ray, out _hit, distance))
        {
            Debug.Log($"{_hit.collider.name}に着弾");
            return true;  
        }
        return false;
    }

    /// <summary>発砲時に呼ぶ</summary>
    public void Fire(GameObject go)
    {
        _isFired = true;
        _rb.velocity = (_speed * transform.forward);
        _go = go;
    }


    /// <summary>着弾時に呼ぶ</summary>
    /// <param name="go"></param>
    void Hit(GameObject go)
    {
        //OnHit();
        go.GetComponent<CharacterBase>()?.Shot(_power);
        Destroy(gameObject);
    }
}
