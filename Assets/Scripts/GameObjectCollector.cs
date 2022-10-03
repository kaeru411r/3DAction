using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �I�u�W�F�N�g���܂Ƃ߂�
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
    /// �n����Transform�̐e�����̃I�u�W�F�N�g��p�̎��W�I�u�W�F�N�g�ɂ���
    /// </summary>
    /// <param name="go"></param>
    protected void Collection(Transform go)
    {
        go.transform.SetParent(StockTransform);
    }
}
