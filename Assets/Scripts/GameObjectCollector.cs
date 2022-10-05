using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �I�u�W�F�N�g���܂Ƃ߂�
/// </summary>
public class GameObjectCollector
{
    public GameObjectCollector(string name)
    {
        _stockTransform = new GameObject().transform;
        _stockTransform.name = $"{name}Objects";
    }

    Transform _stockTransform;

    /// <summary>�X�g�b�N���Ă���Transform</summary>
    public Transform StockTransform { get => _stockTransform; set => _stockTransform = value; }


    /// <summary>
    /// �n����Transform�̐e�����̃I�u�W�F�N�g��p�̎��W�I�u�W�F�N�g�ɂ���
    /// </summary>
    /// <param name="transform"></param>
    public void Collection(Transform transform)
    {
        transform.SetParent(StockTransform);
    }

    /// <summary>
    /// �n����Transform�̐e�����̃I�u�W�F�N�g��p�̎��W�I�u�W�F�N�g�ɂ���
    /// </summary>
    /// <param name="go"></param>
    public void Collection(GameObject go)
    {
        go.transform.SetParent(StockTransform);
    }

    /// <summary>
    /// �n����Transform�̐e�����̃I�u�W�F�N�g��p�̎��W�I�u�W�F�N�g�ɂ���
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
