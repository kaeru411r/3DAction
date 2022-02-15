using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の火器管制コンポーネント
/// </summary>

[RequireComponent(typeof(GunController))]
public class EnemyFireController : MonoBehaviour
{
    /// <summary>このオブジェクトのGunController</summary>
    GunController _gunController;

    [Tooltip("射撃時の精度")]
    [SerializeField] float _accuracy;

    /// <summary>標的のTransform</summary>
    Transform _target;
    /// <summary>サイトオブジェクトのトランスフォーム</summary>
    Transform _sight;


    // Start is called before the first frame update
    void Start()
    {
        _target = PlayerController.Instance.transform;
        _gunController = GetComponent<GunController>();
        _sight = _gunController.Sight;
    }

    // Update is called once per frame
    void Update()
    {
        _sight.LookAt(_target);
    }
}
