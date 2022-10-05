using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトをまとめる
/// </summary>
public class GameObjectCollector
{
    public GameObjectCollector(string name)
    {
        _stockTransform = new GameObject().transform;
        _stockTransform.name = $"{name}Objects";
    }

    Transform _stockTransform;

    /// <summary>ストックしておくTransform</summary>
    public Transform StockTransform { get => _stockTransform; set => _stockTransform = value; }


    /// <summary>
    /// 渡したTransformの親をこのオブジェクト専用の収集オブジェクトにする
    /// </summary>
    /// <param name="transform"></param>
    public void Collection(Transform transform)
    {
        transform.SetParent(StockTransform);
    }

    /// <summary>
    /// 渡したTransformの親をこのオブジェクト専用の収集オブジェクトにする
    /// </summary>
    /// <param name="go"></param>
    public void Collection(GameObject go)
    {
        go.transform.SetParent(StockTransform);
    }

    /// <summary>
    /// 渡したTransformの親をこのオブジェクト専用の収集オブジェクトにする
    /// </summary>
    /// <param name="component"></param>
    public void Collection(Component component)
    {
        component.transform.SetParent(StockTransform);
    }

    public static explicit operator Transform(GameObjectCollector data)
    {
        return data.StockTransform;
    } 

    public static implicit operator bool(GameObjectCollector data)
    {
        return data != null;
    }
}
