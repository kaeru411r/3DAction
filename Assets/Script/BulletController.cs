using UnityEngine;

/// <summary>
/// 砲弾の発射以降の操作を行うコンポーネント
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class BulletController : MonoBehaviour
{
    Rigidbody _rb;
    /// <summary>弾が放たれたかどうか</summary>
    bool _isFired;
    [Tooltip("初速")]
    [SerializeField] float _speed;
    [Tooltip("威力")]
    [SerializeField] float _damage;
    [Tooltip("爆発のダメージ")]
    [SerializeField] float _explosionDamage;
    [Tooltip("爆発の半径")]
    [SerializeField] float _explosionRasius;
    [Tooltip("弾が消滅するまでの時間")]
    [SerializeField] float _destroyTime;
    [Tooltip("リロードにかかる時間")]
    [SerializeField] float _reloadTime;
    [Tooltip("弾にかかる重力")]
    [SerializeField] float _gravity;
    /// <summary>前物理フレームでの座標</summary>
    Vector3 _lastPosition;
    /// <summary>着弾した相手</summary>
    RaycastHit _hit;
    /// <summary>リロードにかかる時間</summary>
    public float ReloadTime { get { return _reloadTime; } }
    /// <summary>発射した戦車</summary>
    Transform _root;
    /// <summary>発射された位置</summary>
    Vector3 _firstPosition;
    /// <summary>発射された時間</summary>
    float _firstTime;

    /// <summary>砲口初速</summary>
    public float Speed { get { return _speed; } }
    /// <summary>重力加速度</summary>
    public float Gravity { get { return _gravity; } }



    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Destroy(gameObject, _destroyTime);
    }


    private void Update()
    {
        if (_isFired)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y - _gravity * Time.deltaTime, _rb.velocity.z);
            transform.forward = _rb.velocity;
            if (HitCheck())   //ここにレイで着弾を観測する部分を書く
            {
                Hit(_hit.transform);
            }
        }
    }

    private void FixedUpdate()
    {
        if (_isFired)
        {
            //_rb.AddForce(0, -_gravity, 0, ForceMode.Acceleration);
        }
    }

    /// <summary>着弾の有無、及びその対象の確認</summary>
    private bool HitCheck()
    {
        var vector = transform.position - _lastPosition;
        var distance = vector.magnitude;
        var direction = vector / distance;
        RaycastHit[] rays;
        rays = Physics.RaycastAll(_lastPosition, direction, distance);
        _lastPosition = transform.position;
        foreach (var r in rays)
        {
            if (r.collider.transform.root != _root)
            {
                _hit = r;
                return true;
            }
        }
        return false;
    }


    /// <summary>発砲時に呼ぶ</summary>
    public void Fire(Transform root)
    {
        if (!_rb)
        {
            _rb = GetComponent<Rigidbody>();
        }
        _isFired = true;
        _rb.velocity = (_speed * transform.forward);
        _root = root;
        _lastPosition = transform.position;
        _firstPosition = transform.position;
        _rb.useGravity = false;
        _firstTime = Time.time;
    }


    /// <summary>着弾時に呼ぶ</summary>
    /// <param name="go"></param>
    void Hit(Transform t)
    {
        Debug.Log($"{t.name}に着弾　高低差{_hit.point.y - _firstPosition.y}" +
            $"　水平距離{Vector2.Distance(new Vector2(_firstPosition.x, _firstPosition.y), new Vector2(_hit.point.x, _hit.point.z))}" +
            $"　相対距離{Vector3.Distance(_firstPosition, _hit.point)} " +
            $"  飛翔時間{Time.time - _firstTime}");
        t.GetComponent<CharacterBase>()?.Shot(_damage);
        ExplosionManager.Instance.Explosion(0, _hit.point, _explosionRasius, _explosionDamage);
        Destroy(gameObject);
    }

    public override string ToString()
    {
        return $"name :{name}, position :{transform.position}";
    }

}
