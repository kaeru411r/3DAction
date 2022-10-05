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
        _name = name;
    }

    string _name;
    Transform _stockTransform;

    Transform StockTransform
    {
        get
        {
            if (!_stockTransform)
            {
                _stockTransform = new GameObject().transform;
                _stockTransform.name = $"{_name}Objects";
            }
            return _stockTransform;
        }
    }

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
}
