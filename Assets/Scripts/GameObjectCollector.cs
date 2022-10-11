using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

/// <summary>
/// オブジェクトをまとめる
/// </summary>
public class GameObjectCollector
{
    public GameObjectCollector()
    {
        _stockTransform = new GameObject(nameof(GameObjectCollector)).transform;
    }
    public GameObjectCollector(string name)
    {
        _stockTransform = new GameObject(name).transform;
    }
    public GameObjectCollector(Transform parent)
    {
        _stockTransform = new GameObject(nameof(GameObjectCollector)).transform;
        _stockTransform.SetParent(parent);
    }
    public GameObjectCollector(string name, Transform parent)
    {
        _stockTransform = new GameObject(name).transform;
        _stockTransform.SetParent(parent);
    }

    ~GameObjectCollector()
    {
        GameObject.Destroy(_stockTransform.gameObject);
    }

    Transform _stockTransform;

    /// <summary>ストックしておくTransform</summary>
    public Transform StockTransform { get => _stockTransform;}


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


    /// <summary>
    /// 子オブジェクトを開放する
    /// </summary>
    public void Release()
    {
        while (_stockTransform.childCount > 0)
        {
            _stockTransform.DetachChildren();
        }
    }


    /// <summary>
    /// ゲームオブジェクトをDestroyする
    /// </summary>
    public void Clear()
    {
        while(_stockTransform.childCount > 0)
        {
            GameObject.Destroy(_stockTransform.GetChild(0).gameObject);
        }
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
