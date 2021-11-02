using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : CharacterBase
{
    /// <summary>プレイヤーのリジッドボディ</summary>
    Rigidbody _rb;
    [Tooltip("移動速度")]
    [SerializeField] float _speed;
    [SerializeField] BulletController[] _bullets;
    BulletController _bullet;


    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _bullet = _bullets[0];
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 vector = new Vector3(x, 0, z);
        vector = vector.normalized;
        _rb.AddForce(vector, ForceMode.Impulse);
    }

    void Fire()
    {
        var go = Instantiate(_bullet, transform.position, transform.rotation);
        go.GetComponent<BulletController>()?.Fire(gameObject);
    }

    void Choice()
    {

    }
}
