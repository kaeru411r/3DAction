using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

/// <summary>
/// �I�u�W�F�N�g���܂Ƃ߂�
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

    /// <summary>�X�g�b�N���Ă���Transform</summary>
    public Transform StockTransform { get => _stockTransform;}


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


    /// <summary>
    /// �q�I�u�W�F�N�g���J������
    /// </summary>
    public void Release()
    {
        while (_stockTransform.childCount > 0)
        {
            _stockTransform.DetachChildren();
        }
    }


    /// <summary>
    /// �Q�[���I�u�W�F�N�g��Destroy����
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
