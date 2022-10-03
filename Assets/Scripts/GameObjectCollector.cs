using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトをまとめる
/// </summary>
abstract public class GameObjectCollector : MonoBehaviour
{

    Transform _stockTransform;
    Transform StockTransform
    {
        get
        {
            if (!_stockTransform)
            {
                _stockTransform = new GameObject().transform;
                _stockTransform.name = $"{name}Objects";
            }
            return _stockTransform;
        }
    }

    /// <summary>
    /// 渡したTransformの親をこのオブジェクト専用の収集オブジェクトにする
    /// </summary>
    /// <param name="go"></param>
    protected void Collection(Transform go)
    {
        go.transform.SetParent(StockTransform);
    }
}
