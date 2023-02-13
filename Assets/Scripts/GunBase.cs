using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class GunBase : MonoBehaviour
{
    /// <summary>�o����</summary>
    abstract public Transform Barrel { get; }
    /// <summary>�}�Y��</summary>
    abstract public Transform Muzzle { get; }
    /// <summary>���ݑI�𒆂̒e��</summary>
    abstract public BaseBullet Bullet { get; }

    /// <summary>�C���֐�</summary>
    /// <returns>�������C�e�̔z��</returns>
    abstract public BaseBullet[] Fire();
}
