using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterBase : MonoBehaviour
{
    [Tooltip("HP")]
    [SerializeField] float _hp;


    public void Shot(float damage)
    {
        _hp -= damage;
        Debug.Log($"{damage}のダメージ");
        if (_hp <= 0)
            Death();
    }

    void Death()
    {
        Debug.Log($"{name}はやられた");
        Destroy(gameObject);
    }
}
