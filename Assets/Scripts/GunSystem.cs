using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor.Timeline.Actions;
using UnityEngine;

/// <summary>
/// ������Gun���ЂƂ̃C���X�^���X���̂悤�ɐ��䂷�邽�߂̃R���|�[�l���g
/// </summary>
public class GunSystem : MonoBehaviour
{

    [Tooltip("�C�z��")]
    [SerializeField] List<Gun> _guns;
    [Tooltip("�C�̎ˌ��p�^�[��")]
    [SerializeField] FireTimingMode _fireTimingMode;

    /// <summary>���C�̔ԍ�</summary>
    int _gunNumber;
    float _coolTime;

    /// <summary>�C</summary>
    public Gun Gun { get => _guns.FirstOrDefault(); }
    /// <summary>�C�e</summary>
    public Bullet Bullet { get => Gun.Bullet; }
    /// <summary>�C�g</summary>
    public Transform Barrel { get => Gun.Barrel; }
    /// <summary>�C��</summary>
    public Transform Muzzle { get => Gun.Muzzle; }
    /// <summary>�C�̎ˌ��p�^�[��</summary>
    public FireTimingMode FireTimingMode { get => _fireTimingMode; set => _fireTimingMode = value; }

    /// <summary>���C�̔ԍ�</summary>
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



    /// <summary>�C�e�̎��̉����甭�ˊ֐��̌Ăяo���܂ł��s��</summary>
    /// <returns>���C�����C�e�Q ���s���Ă�����null</returns>
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


    /// <summary>�C�e�̐؂�ւ����s��</summary>
    /// <param name="f"></param>
    public bool Change(float f)
    {
        return Gun.Change(f);
    }

    /// <summary>�C�e�̑I�����s��</summary>
    /// <param name="n"></param>
    public bool Choice(int n)
    {
        return Gun.Choice(n);
    }
}

public enum FireTimingMode
{
    /// <summary>����</summary>
    Coinstantaneous,
    /// <summary>�A��</summary>
    Concatenation,
    /// <summary>�S�Ă̖C��S�͂Ō���</summary>
    FullOpen,
}