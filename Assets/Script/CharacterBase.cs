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

    /// <summary>スタート関数で必ず呼ぶ</summary>
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
    /// <param name="x"></param>
    /// <param name="z"></param>
    public void Move(float x, float z)
    {
        Vector3 vector = new Vector3(x, 0, z);
        vector = vector.normalized;
        _rb.AddForce(vector, ForceMode.Impulse);
    }
}
