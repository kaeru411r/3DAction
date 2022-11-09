using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーが操作をするためのクラスのベースクラス
/// プレイヤー操作が有効な時のみ各コンポーネントの関数を実行できる関数を持っている
/// </summary>
public class Player : MonoBehaviour
{

    /// <summary>マウス移動</summary>
    public void OnInput(InputAction.CallbackContext context)
    {
    }
    protected bool _isActive;

    public bool IsActive { get => _isActive; set => _isActive = value; }

    /// <summary>
    /// 操作が有効な時のみメソッドを実行する
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
    /// 操作が有効な時のみメソッドを実行する
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
    /// 操作が有効な時のみ変数に代入する
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
