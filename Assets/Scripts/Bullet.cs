using Unity.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

/// <summary>
/// 砲弾の発射以降の操作を行うコンポーネント
/// 原則RigidBodyは利用しない
/// </summary>
//[RequireComponent(typeof(Rigidbody))]
public class Bullet : BaseBullet
{
    //Rigidbody _rb;
    [Tooltip("初速")]
    [SerializeField] float _firstSpeed;
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
    [Tooltip("砲弾の質量")]
    [SerializeField] float _mass = 1;
    [Tooltip("弾にかかる重力")]
    [SerializeField] float _gravity;
    [ReadOnly, Tooltip("弾体の速度")]
    [SerializeField] Vector3 _velocity;
    /// <summary>前物理フレームでの座標</summary>
    Vector3 _lastPosition;
    /// <summary>着弾した相手</summary>
    RaycastHit _hit;
    /// <summary>発射した戦車</summary>
    Transform _root;
    /// <summary>発射された位置</summary>
    Vector3 _firstPosition;
    /// <summary>発射された時間</summary>
    float _firstTime;
    /// <summary>弾が放たれたかどうか</summary>
    bool _isFired;

    /// <summary>リロードにかかる時間</summary>
    public override float ReloadTime { get { return _reloadTime; } }
    /// <summary>砲口初速</summary>
    public override float Speed { get { return _firstSpeed; } }
    /// <summary>砲弾の質量</summary>
    public override float Mass { get => _mass;}
    /// <summary>重力加速度</summary>
    public override float Gravity { get { return _gravity; } }
    /// <summary>弾体の速度</summary>
    public Vector3 Velocity { get => _velocity; set => _velocity = value; }

    private void Start()
    {
        //_rb = GetComponent<Rigidbody>();
        Destroy(gameObject, _destroyTime);
    }


    private void Update()
    {
        transform.forward = Velocity;
    }

    private void FixedUpdate()
    {
        Velocity += Vector3.down * _gravity * Time.fixedDeltaTime;
        FixedMove(Time.fixedDeltaTime);
        if (_isFired)
        {
            //_rb.AddForce(Vector3.down * _gravity, ForceMode.Acceleration);
            if (HitCheck())   //ここにレイで着弾を観測する部分を書く
            {
                Hit(_hit.transform);
            }
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
    override public GameObject Fire(Transform root)
    {
        //if (!_rb)
        //{
        //    _rb = GetComponent<Rigidbody>();
        //}
        _isFired = true;
        _velocity = (_firstSpeed * transform.forward);
        _root = root;
        _lastPosition = transform.position;
        _firstPosition = transform.position;
        //_rb.useGravity = false;
        _firstTime = Time.time;
        return gameObject;
    }


    /// <summary>着弾時に呼ぶ</summary>
    /// <param name="go"></param>
    void Hit(Transform t)
    {
        Debug.Log($"{t.name}に着弾　高低差{_hit.point.y - _firstPosition.y}" +
            $"　水平距離{Vector2.Distance(new Vector2(_firstPosition.x, _firstPosition.y), new Vector2(_hit.point.x, _hit.point.z))}" +
            $"　相対距離{Vector3.Distance(_firstPosition, _hit.point)} " +
            $"  飛翔時間{Time.time - _firstTime}" +
            $"　着弾位置{t.position}");
        t.GetComponent<CharacterBase>()?.Shot(_damage);
        ExplosionManager.Instance.Explosion(0, _hit.point, _explosionRasius, _explosionDamage);
        Destroy(gameObject);
    }


    void FixedMove(float deltaTime)
    {
        transform.position += _velocity * deltaTime;
    }


    public override string ToString()
    {
        return $"name :{name}, position :{transform.position}";
    }

}