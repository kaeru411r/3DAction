using UnityEngine;


/// <summary>
/// 全キャラクターの共通部分の基底クラス
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class CharacterBase : MonoBehaviour
{
    [Tooltip("HP")]
    [SerializeField] float _hp;
    [Tooltip("移動速度")]
    [SerializeField] float _speed;
    [Tooltip("車両の旋回速度")]
    [SerializeField] float _vehicleTurnSpeed;
    /// <summary>地面についているかどうか</summary>
    bool _isGround;
    /// <summary>キャラクターのリジッドボディ</summary>
    Rigidbody _rb;


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.centerOfMass = new Vector3(0, -1, 0);
    }

    private void OnEnable()
    {
        if (!_rb)
        {
            _rb = GetComponent<Rigidbody>();
        }
        ExplosionManager.Instance.Add(_rb);
    }

    private void OnDisable()
    {
        ExplosionManager.Instance.Remove(_rb);
    }

    private void Update()
    {
        //_rb.centerOfMass = Vector3.zero;
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
    public void Move(Vector2 vector)
    {
        Debug.LogError(vector);
        if (_isGround)
        {
            vector = vector.normalized;
            _rb.AddForce(transform.forward * vector.y * Time.deltaTime * _speed, ForceMode.Impulse);
            var ro = new Vector3(_rb.angularVelocity.x, vector.x * _vehicleTurnSpeed * Time.deltaTime, _rb.angularVelocity.z);
            _rb.angularVelocity = ro;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
            _isGround = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
            _isGround = false;
    }
}
