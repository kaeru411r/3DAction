using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;


/// <summary>
/// ������GunSystem��C���ƂƂ��ɉ^�p���邽�߂̃R���|�[�l���g
/// �C�̏㉺���E�̓�����GunSystem�̐���������Ȃ�
/// </summary>
public class Turret : MonoBehaviour
{
    /// <summary>360�x</summary>
    const int AllAround = 360;

    [Tooltip("���̃^�[���b�g���g�p����GunSystem")]
    [SerializeField] GunSystem[] _gunSystems;
    [Tooltip("�g��GunSystem�̔ԍ�")]
    [SerializeField] int _gunNumber = 0;
    [Tooltip("���̖C����TurretMovement")]
    [SerializeField] TurretMovement _turretMovement;
    public GunSystem GunSystem { get => _gunSystems[GunSystemsNumber]; }

    public Transform Sight { get => _turretMovement.Sight; }

    public Transform Barrel { get => GunSystem.transform; }

    public Transform Muzzle { get => GunSystem.Muzzle; }

    public Vector2 GunMoveSpeed { get => _turretMovement.Speed; }

    public float Misalignment { get=> _turretMovement.Misalignment; }
    public List<Gun> Guns { get => GunSystem.Guns; }
    /// <summary>�C�e�̏d�͉����x</summary>
    public Vector3 Gravity { get => GunSystem.Gravity; }
    /// <summary>�e��</summary>
    public float Speed { get => GunSystem.Speed; }
    /// <summary>�g��GunSystem�̔ԍ�</summary>
    public int GunSystemsNumber { get => _gunNumber;
        set
        {
            if(value < 0 || value >= _gunSystems.Length)
            {
                Debug.LogWarning($"���݂��Ȃ�index�ł�");
                return;
            }
            _gunNumber = value;
        }
    }

    /// <summary>�C�e�̎��̉����甭�ˊ֐��̌Ăяo���܂ł��s��</summary>
    /// <returns>���˂����C�e�Q ���C�Ɏ��s���Ă�����null</returns>
    public List<BaseBullet> Fire()
    {
        return GunSystem.Fire();
    }
}
