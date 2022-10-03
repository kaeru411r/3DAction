using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GunSystem : MonoBehaviour
{

    [Tooltip("–C”z—ñ")]
    [SerializeField] List<Gun> _guns;

    public Gun Gun { get => _guns.FirstOrDefault(); }

    public Bullet Bullet { get => Gun.Bullet; }

    public Transform Barrel { get => Gun.Barrel; }

    public Transform Muzzle { get => Gun.Muzzle; }



    /// <summary>–C’e‚ÌÀ‘Ì‰»‚©‚ç”­ËŠÖ”‚ÌŒÄ‚Ño‚µ‚Ü‚Å‚ğs‚¤</summary>
    /// <param name="root"></param>
    /// <returns>”­–C‚µ‚½‚©”Û‚©</returns>
    public bool Fire()
    {
        return Gun.Fire();
    }

    /// <summary>–C’e‚ÌØ‚è‘Ö‚¦‚ğs‚¤</summary>
    /// <param name="f"></param>
    public bool Change(float f)
    {
        return Gun.Change(f);
    }

    /// <summary>–C’e‚Ì‘I‘ğ‚ğs‚¤</summary>
    /// <param name="n"></param>
    public bool Choice(int n)
    {
        return Gun.Choice(n);
    }


}
