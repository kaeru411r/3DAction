using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class GunBase : MonoBehaviour
{
    /// <summary>ƒoƒŒƒ‹</summary>
    abstract public Transform Barrel { get; }
    /// <summary>ƒ}ƒYƒ‹</summary>
    abstract public Transform Muzzle { get; }
    /// <summary>Œ»İ‘I‘ğ’†‚Ì’e–ò</summary>
    abstract public BaseBullet Bullet { get; }

    /// <summary>–CŒ‚ŠÖ”</summary>
    /// <returns>Œ‚‚Á‚½–C’e‚Ì”z—ñ</returns>
    abstract public BaseBullet[] Fire();
}
