using UnityEngine;


/// <summary>
/// 全キャラクターの共通部分の基底クラス
/// </summary>
public class CharacterBase : MonoBehaviour
{
    [Tooltip("HP")]
    [SerializeField] float _hp;
    /// <summary>キャラクターのリジッドボディ</summary>
    Rigidbody _rb;
    /// <summary>このインスタンスが有効か否か</summary>
    bool _isSleeping = false;

    /// <summary>このインスタンスが有効か否か</summary>
    public bool IsSleeping { get { return _isSleeping; } }

    public float Hp { get { return _hp; } }



    private void OnEnable()
    {
        _isSleeping = false;
        if (!_rb)
        {
            _rb = GetComponent<Rigidbody>();
            ExplosionManager.Instance.Add(_rb);
        }
        else
        {
            ExplosionManager.Instance.Add(_rb);
        }
        ExplosionManager.Instance.Add(this);
        ExplosionManager.Instance.Add(_rb);
    }

    private void OnDisable()
    {
        _isSleeping = true;
    }

    /// <summary>被弾時の処理</summary>
    /// <param name="damage"></param>
    public void Shot(float damage)
    {
        if(!_isSleeping)
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
}
