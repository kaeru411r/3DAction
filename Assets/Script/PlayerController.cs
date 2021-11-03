using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// プレイヤー操作コンポーネント
/// 車両の操作を行う
/// </summary>
public class PlayerController : CharacterBase
{
    /// <summary>プレイヤーのリジッドボディ</summary>
    Rigidbody _rb;
    [Tooltip("移動速度")]
    [SerializeField] float _speed;


    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 vector = new Vector3(x, 0, z);
        vector = vector.normalized;
        _rb.AddForce(vector, ForceMode.Impulse);
    }
}
