using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tester : CharacterBase
{
    [SerializeField] GameObject _bullet;

    public void Fire()
    {
        var go = Instantiate(_bullet, transform.position, transform.rotation);
        go.GetComponent<BulletController>()?.Fire(transform);
    }
    
}
