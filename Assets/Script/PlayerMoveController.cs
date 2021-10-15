using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    [Tooltip("移動速度")]
    [SerializeField] float m_speed;
    /// <summary>プレイヤーのリジッドボディ</summary>
    Rigidbody m_rb;

    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
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
        m_rb.AddForce(vector, ForceMode.Impulse);
    }
}
