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
    Transform _target;
    [Tooltip("サイト")]
    [SerializeField] Transform _sight;


    // Start is called before the first frame update
    void Start()
    {
        _target = PlayerController.Instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
