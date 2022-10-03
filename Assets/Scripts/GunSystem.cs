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

    public Gun Gun { get => _guns.FirstOrDefault(); }

    public Bullet Bullet { get => Gun.Bullet; }

    public Transform Barrel { get => Gun.Barrel; }

    public Transform Muzzle { get => Gun.Muzzle; }



    /// <summary>�C�e�̎��̉����甭�ˊ֐��̌Ăяo���܂ł��s��</summary>
    /// <param name="root"></param>
    /// <returns>���C�������ۂ�</returns>
    public bool Fire()
    {
        return Gun.Fire();
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
}