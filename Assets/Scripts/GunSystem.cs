using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor.Timeline.Actions;
using UnityEngine;

/// <summary>
/// 複数のGunをひとつのインスタンスかのように制御するためのコンポーネント
/// </summary>
public class GunSystem : MonoBehaviour
{

    [Tooltip("砲配列")]
    [SerializeField] List<Gun> _guns;
    [Tooltip("砲の射撃パターン")]
    [SerializeField] FireTimingMode _fireTimingMode;

    public Gun Gun { get => _guns.FirstOrDefault(); }

    public Bullet Bullet { get => Gun.Bullet; }

    public Transform Barrel { get => Gun.Barrel; }

    public Transform Muzzle { get => Gun.Muzzle; }



    /// <summary>砲弾の実体化から発射関数の呼び出しまでを行う</summary>
    /// <param name="root"></param>
    /// <returns>発砲したか否か</returns>
    public bool Fire()
    {
        return Gun.Fire();
    }

    /// <summary>砲弾の切り替えを行う</summary>
    /// <param name="f"></param>
    public bool Change(float f)
    {
        return Gun.Change(f);
    }

    /// <summary>砲弾の選択を行う</summary>
    /// <param name="n"></param>
    public bool Choice(int n)
    {
        return Gun.Choice(n);
    }


}

public enum FireTimingMode
{
    /// <summary>同時</summary>
    Coinstantaneous,
    /// <summary>連続</summary>
    Concatenation,
}