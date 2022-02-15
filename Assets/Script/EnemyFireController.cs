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
        if (_target)
        {
            _sight.LookAt(_target);
            Vector3 relativePosition = _target.position - _sight.position;
            Vector3 angle = Quaternion.Euler(relativePosition) * Vector3.forward;
            float misalignment = (_gunController.Barrel.eulerAngles - _sight.eulerAngles).magnitude;
            misalignment = misalignment < 180 ? misalignment : Mathf.Abs(misalignment - 360);

            if (misalignment <= _accuracy)
            {
                _gunController.Fire();
            }
        }
    }


}
