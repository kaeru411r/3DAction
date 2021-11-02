using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [Tooltip("HP")]
    [SerializeField] float _hp;
    [Tooltip("移動速度")]
    [SerializeField] float _speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shot(float damage)
    {
        _hp -= damage;
        Debug.Log($"{damage}のダメージ");
    }

    
}
