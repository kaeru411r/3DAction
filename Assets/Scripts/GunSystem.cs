using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor.Timeline.Actions;
using UnityEngine;

/// <summary>
/// •¡”‚ÌGun‚ğ‚Ğ‚Æ‚Â‚ÌƒCƒ“ƒXƒ^ƒ“ƒX‚©‚Ì‚æ‚¤‚É§Œä‚·‚é‚½‚ß‚ÌƒRƒ“ƒ|[ƒlƒ“ƒg
/// </summary>
public class GunSystem : MonoBehaviour
{

    [Tooltip("–C”z—ñ")]
    [SerializeField] List<Gun> _guns;
    [Tooltip("–C‚ÌËŒ‚ƒpƒ^[ƒ“")]
    [SerializeField] FireTimingMode _fireTimingMode;

    /// <summary>Œ‚‚Â–C‚Ì”Ô†</summary>
    int _gunNumber;
    float _coolTime;

    /// <summary>–C</summary>
    public Gun Gun { get => _guns.FirstOrDefault(); }
    /// <summary>–C’e</summary>
    public Bullet Bullet { get => Gun.Bullet; }
    /// <summary>–Cg</summary>
    public Transform Barrel { get => Gun.Barrel; }
    /// <summary>–CŒû</summary>
    public Transform Muzzle { get => Gun.Muzzle; }
    /// <summary>–C‚ÌËŒ‚ƒpƒ^[ƒ“</summary>
    public FireTimingMode FireTimingMode { get => _fireTimingMode; set => _fireTimingMode = value; }

    /// <summary>Œ‚‚Â–C‚Ì”Ô†</summary>
    int GunNumber
    {
        get
        {
            return _gunNumber;
        }
        set
        {
            if (value >= _guns.Count)
            {
                value = 0;
            }
            _gunNumber = value;
        }
    }

    private void FixedUpdate()
    {
        _coolTime -= Time.fixedDeltaTime;
    }



    /// <summary>–C’e‚ÌÀ‘Ì‰»‚©‚ç”­ËŠÖ”‚ÌŒÄ‚Ño‚µ‚Ü‚Å‚ğs‚¤</summary>
    /// <returns>”­–C‚µ‚½–C’eŒQ ¸”s‚µ‚Ä‚¢‚½‚çnull</returns>
    public List<Bullet> Fire()
    {
        if (_fireTimingMode == FireTimingMode.Coinstantaneous)
        {
            if (IsAllReload())
            {
                List<Bullet> bullets = new List<Bullet>();
                for (int i = 0; i < _guns.Count; i++)
                {
                    Bullet b = _guns[i].Fire();
                    if (b)
                    {
                        bullets.Add(b);
                    }
                }
                GunNumber = 0;
                return bullets;
            }
        }
        else if (FireTimingMode == FireTimingMode.Concatenation)
        {
            if (_coolTime <= 0)
            {
                Bullet b = _guns[GunNumber].Fire();
                if (b)
                {
                    List<Bullet> bullets = new List<Bullet>();
                    bullets.Add(b);
                    _coolTime += _guns.Max(g => g.Bullet.ReloadTime) / _guns.Count;
                    GunNumber++;
                    return bullets;
                }
            }
        }
        else if (_fireTimingMode == FireTimingMode.FullOpen)
        {
            List<Bullet> bullets = new List<Bullet>();
            for (int i = 0; i < _guns.Count; i++)
            {
                Bullet b = _guns[i].Fire();
                if (b)
                {
                    bullets.Add(b);
                }
            }
            GunNumber = 0;
            return bullets;
        }
        else { }
        return null;
    }

    bool IsAllReload()
    {
        bool isLoad = true;
        foreach (Gun gun in _guns)
        {
            if (!gun.IsLoad)
            {
                isLoad = false;
                break;
            }
        }
        return isLoad;
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

public enum FireTimingMode
{
    /// <summary>“¯</summary>
    Coinstantaneous,
    /// <summary>˜A‘±</summary>
    Concatenation,
    /// <summary>‘S‚Ä‚Ì–C‚ğ‘S—Í‚ÅŒ‚‚Â</summary>
    FullOpen,
}