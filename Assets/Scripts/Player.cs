using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// �v���C���[����������邽�߂̃N���X�̃x�[�X�N���X
/// �v���C���[���삪�L���Ȏ��̂݊e�R���|�[�l���g�̊֐������s�ł���֐��������Ă���
/// </summary>
public class Player : MonoBehaviour
{

    /// <summary>�}�E�X�ړ�</summary>
    public void OnInput(InputAction.CallbackContext context)
    {
    }
    protected bool _isActive;

    public bool IsActive { get => _isActive; set => _isActive = value; }

    /// <summary>
    /// ���삪�L���Ȏ��̂݃��\�b�h�����s����
    /// </summary>
    /// <typeparam name="Input"></typeparam>
    /// <typeparam name="Return"></typeparam>
    /// <param name="value"></param>
    /// <param name="method"></param>
    /// <returns></returns>
    protected Return Call<Input, Return>(Input value, Func<Input, Return> method) 
    {
        if (!_isActive)
        {
            return default;
        }
        return method(value);
    }

    /// <summary>
    /// ���삪�L���Ȏ��̂݃��\�b�h�����s����
    /// </summary>
    /// <typeparam name="Input"></typeparam>
    /// <typeparam name="Return"></typeparam>
    /// <param name="value"></param>
    /// <param name="method"></param>
    /// <returns></returns>
    protected Return Call<Return>(Func<Return> method)
    {
        if (!_isActive)
        {
            return default;
        }
        return method();
    }

    /// <summary>
    /// ���삪�L���Ȏ��̂ݕϐ��ɑ������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    protected bool Set<T>(ref T a, in T b)
    {
        if (!_isActive)
        {
            return false;
        }
        a = b;
        return true;
    }
}
