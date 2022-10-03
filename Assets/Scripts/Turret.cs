using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;


/// <summary>
/// 複数のGunSystemを砲塔とともに運用するためのコンポーネント
/// 砲の上下左右の動きとGunSystemの制御をおこなう
/// </summary>
public class Turret : MonoBehaviour
{
    /// <summary>360度</summary>
    const int AllAround = 360;

    [Tooltip("直接照準を合わせる基準")]
    [SerializeField] Transform _sight;
    [Tooltip("砲の動作スピード[rad/s]")]
    [SerializeField] Vector2 _gunMoveSpeed;
    [Tooltip("仰角")]
    [SerializeField, Range(0, 90)] float _elevationAngle = 0;
    [Tooltip("俯角")]
    [SerializeField, Range(-90, 0)] float _depressionAngle = 0;
    [Tooltip("このターレットが使用するGunSystem")]
    [SerializeField] GunSystem[] _gunSystems;

    public GunSystem GunSystem { get => _gunSystems.FirstOrDefault(); }

    public Transform Sight { get => _sight; }

    public Transform Barrel { get => GunSystem.Barrel; }

    public Transform Muzzle { get => GunSystem.Muzzle; }

    public Vector2 GunMoveSpeed { get => _gunMoveSpeed; }
    public Gun Gun { get => GunSystem.Gun; }

    public Bullet Bullet { get => GunSystem.Bullet; }
    // Start is called before the first frame update
    void Start()
    {
        if (!_sight)
        {
            Debug.LogError($"{name}は{nameof(_sight)}がアサインされていません");
        }
    }

    private void FixedUpdate()
    {
        Yaw(_sight.localEulerAngles.y, Time.fixedDeltaTime);
        Pitch(_sight.localEulerAngles.x, Time.fixedDeltaTime);
        float x = _sight.localEulerAngles.x;
        if (x > AllAround / 2)
        {
            x -= AllAround;
        }
        if (x < -_elevationAngle)
        {
            //Debug.Log($"#1 {_sight.eulerAngles.x} {-_elevationAngle} {_sight.eulerAngles.x < -_elevationAngle}");
            _sight.localEulerAngles = new Vector3(-_elevationAngle, _sight.localEulerAngles.y, _sight.localEulerAngles.z);
        }
        else if (x > -_depressionAngle)
        {
            //Debug.Log($"#2 {_sight.eulerAngles.x} {-_depressionAngle} {_sight.eulerAngles.x > _depressionAngle}");
            _sight.localEulerAngles = new Vector3(-_depressionAngle, _sight.localEulerAngles.y, _sight.localEulerAngles.z);
        }
    }


#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_gunMoveSpeed.x < 0)
        {
            _gunMoveSpeed = new Vector2(0, _gunMoveSpeed.y);
        }
        if (_gunMoveSpeed.y < 0)
        {
            _gunMoveSpeed = new Vector2(_gunMoveSpeed.y, 0);
        }
    }

#endif

    /// <summary>砲身の上下の動き</summary>
    /// <param name="x"></param>
    void Pitch(float x, float deltaTime)
    {
        var dif = x - Barrel.localEulerAngles.x;
        if (dif < -AllAround / 2)
        {
            dif = dif + AllAround;
        }
        else if (dif > AllAround / 2)
        {
            dif = dif - AllAround;
        }
        if (dif <= _gunMoveSpeed.y && dif >= -_gunMoveSpeed.y * deltaTime / Mathf.PI * 180)
        {
            Barrel.localEulerAngles = new Vector3(x, 0, 0);
        }
        else if (dif > _gunMoveSpeed.y)
        {
            Barrel.Rotate(_gunMoveSpeed.y * deltaTime / Mathf.PI * 180, 0, 0);
        }
        else
        {
            Barrel.Rotate(-_gunMoveSpeed.y * deltaTime / Mathf.PI * 180, 0, 0);
        }
    }

    /// <summary>砲塔の旋回</summary>
    /// <param name="y"></param>
    void Yaw(float y, float deltaTime)
    {
        var dif = y - transform.localEulerAngles.y;
        if (dif < -AllAround / 2)
        {
            dif = dif + AllAround;
        }
        else if (dif > AllAround / 2)
        {
            dif = dif - AllAround;
        }
        if (dif <= _gunMoveSpeed.x && dif >= -_gunMoveSpeed.x * deltaTime / Mathf.PI * 180)
        {
            transform.localEulerAngles = new Vector3(0, y, 0);
        }
        else if (dif > _gunMoveSpeed.x)
        {
            transform.Rotate(0, _gunMoveSpeed.x * deltaTime / Mathf.PI * 180, 0);
        }
        else
        {
            transform.Rotate(0, -_gunMoveSpeed.x * deltaTime / Mathf.PI * 180, 0);
        }
    }

    /// <summary>砲弾の実体化から発射関数の呼び出しまでを行う</summary>
    /// <param name="root"></param>
    /// <returns>発砲したか否か</returns>
    public bool Fire()
    {
        return GunSystem.Fire();
    }

    /// <summary>砲弾の切り替えを行う</summary>
    /// <param name="f"></param>
    public bool Change(float f)
    {
        return GunSystem.Change(f);
    }

    /// <summary>砲弾の選択を行う</summary>
    /// <param name="n"></param>
    public bool Choice(int n)
    {
        return GunSystem.Choice(n);
    }

}
