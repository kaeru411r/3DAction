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

    [Tooltip("このターレットが使用するGunSystem")]
    [SerializeField] GunSystem[] _gunSystems;
    [Tooltip("使うGunSystemの番号")]
    [SerializeField] int _gunNumber = 0;
    [Tooltip("この砲塔のTurretMovement")]
    [SerializeField] TurretMovement _turretMovement;
    public GunSystem GunSystem { get => _gunSystems[GunSystemsNumber]; }

    public Transform Sight { get => _turretMovement.Sight; }

    public Transform Barrel { get => GunSystem.transform; }

    public Transform Muzzle { get => GunSystem.Muzzle; }

    public Vector2 GunMoveSpeed { get => _turretMovement.Speed; }

    public float Misalignment { get=> _turretMovement.Misalignment; }
    public List<Gun> Guns { get => GunSystem.Guns; }
    /// <summary>砲弾の重力加速度</summary>
    public Vector3 Gravity { get => GunSystem.Gravity; }
    /// <summary>弾速</summary>
    public float Speed { get => GunSystem.Speed; }
    /// <summary>使うGunSystemの番号</summary>
    public int GunSystemsNumber { get => _gunNumber;
        set
        {
            if(value < 0 || value >= _gunSystems.Length)
            {
                Debug.LogWarning($"存在しないindexです");
                return;
            }
            _gunNumber = value;
        }
    }

    /// <summary>砲弾の実体化から発射関数の呼び出しまでを行う</summary>
    /// <returns>発射した砲弾群 発砲に失敗していたらnull</returns>
    public List<BaseBullet> Fire()
    {
        return GunSystem.Fire();
    }
}
