using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
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
    [Header("�����艺�͖����Ă��ǂ�")]
    [Tooltip("���ƂȂ��̃}�Y���̕ӂ��Transform")]
    [SerializeField] Transform _muzzle;
    [Tooltip("���ƂȂ��̃o�����̓������Transform")]
    [SerializeField] Transform _barrel;
    /// <summary>���C�̔ԍ�</summary>
    int _gunNumber;
    /// <summary>���̖C���܂ł̃N�[���^�C��</summary>
    float _coolTime;

    /// <summary>�C</summary>
    public List<Gun> Guns { get => _guns; }
    /// <summary>�C�e�̏d�͉����x</summary>
    public float Gravity
    {
        get
        {
            if(_guns == null || _guns.Count == 0)
            {
                return 0;
            }
            if(_guns.Distinct().Count() == 1)
            {
                return _guns[0].Bullet.Gravity;
            }
            else
            {
                return _guns.Sum(g => g.Bullet.Gravity) / _guns.Count;
            }
        }
    }
    /// <summary>�e��</summary>
    public float Speed
    {
        get
        {
            if (_guns == null || _guns.Count == 0)
            {
                return 0;
            }
            if (_guns.Distinct().Count() == 1)
            {
                return _guns[0].Bullet.Speed;
            }
            else
            {
                return _guns.Sum(g => g.Bullet.Speed) / _guns.Count;
            }
        }
    }
    /// <summary>�C�g</summary>
    public Transform Barrel {
        get
        {
            if (!_barrel)
            {
                if (_guns.Count > 1)
                {
                    _barrel = new GameObject().transform;
                    _barrel.name = nameof(_barrel);
                    _barrel.SetParent(transform);
                    float x = _guns.Sum(g => g.Barrel.localPosition.x) / _guns.Count;
                    float y = _guns.Sum(g => g.Barrel.localPosition.y) / _guns.Count;
                    float z = _guns.Sum(g => g.Barrel.localPosition.z) / _guns.Count;
                    _barrel.localPosition = new Vector3(x, y, z);
                }
                else if (_guns.Count == 1)
                {
                    _barrel = _guns[0].Barrel;
                }
                else
                {
                    _barrel = transform;
                }
            }
            return _barrel;
        } 
        set => _barrel = value;
    }
    /// <summary>�C��</summary>
    public Transform Muzzle { 
        get
        {
            if (!_muzzle)
            {
                if (_guns.Count > 1)
                {
                    _muzzle = new GameObject().transform;
                    _muzzle.name = nameof(_muzzle);
                    _muzzle.SetParent(transform);
                    float x = _guns.Sum(g => g.Muzzle.localPosition.x) / _guns.Count;
                    float y = _guns.Sum(g => g.Muzzle.localPosition.y) / _guns.Count;
                    float z = _guns.Sum(g => g.Muzzle.localPosition.z) / _guns.Count;
                    _muzzle.localPosition = new Vector3(x, y, z);
                }
                else if(_guns.Count == 1)
                {
                    _muzzle = _guns[0].Muzzle;
                }
                else
                {
                    _muzzle = transform;
                }
            }
            return _muzzle;
        }
        set => _muzzle = value;
    }
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