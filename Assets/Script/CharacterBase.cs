using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 全キャラクターの共通部分の基底クラス
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class CharacterBase : MonoBehaviour
{
    [Tooltip("HP")]
    [SerializeField] float _hp;
    /// <summary>キャラクターのリジッドボディ</summary>
    Rigidbody _rb;
    [Tooltip("移動速度")]
    [SerializeField] float _speed;
    [Tooltip("車両の旋回速度")] 
    [SerializeField] float _vehicleTurnSpeed;
    bool _isGround;


    public void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    /// <summary>被弾時の処理</summary>
    /// <param name="damage"></param>
    public void Shot(float damage)
    {
        Damage(damage);
    }

    /// <summary>ダメージを受ける</summary>
    /// <param name="damage"></param>
    void Damage(float damage)
    {
        _hp -= damage;
        Debug.Log($"{damage}のダメージ");
        if (_hp <= 0)
            Death();
    }

    /// <summary>破壊される</summary>
    void Death()
    {
        Debug.Log($"{name}はやられた");
        Destroy(gameObject);
    }

    /// <summary>移動</summary>
    /// <param name="z"></param>
    /// <param name="y"></param>
    public void Move(float z, float y)
    {
        if (_isGround)
        {
            Vector2 vector = new Vector2(z, y);
            vector = vector.normalized;
            _rb.AddForce(transform.forward * vector.x * Time.deltaTime * _speed, ForceMode.Impulse);
            var ro = new Vector3(_rb.angularVelocity.x, vector.y * _vehicleTurnSpeed * Time.deltaTime, _rb.angularVelocity.z);
            _rb.angularVelocity = ro;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        _isGround = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
            _isGround = false;
    }
}
