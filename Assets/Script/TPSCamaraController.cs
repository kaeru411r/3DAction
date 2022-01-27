using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

/// <summary>
/// TPSカメラの操作をするクラス
/// </summary>
public class TPSCamaraController : MonoBehaviour
{
    /// <summary>このスクリプトで操作をするVcam</summary>
    CinemachineVirtualCameraBase _vCam;
    [Tooltip("リグの最長半径")]
    [SerializeField] float _maxRadius;
    [Tooltip("カメラ速度")]
    [SerializeField] Vector2 _speed;
    [Tooltip("上部リグの半径と高さ")]
    [SerializeField] Vector2 _topRig;
    [Tooltip("中部リグの半径と高さ")]
    [SerializeField] Vector2 _middleRig;
    [Tooltip("下部リグの半径と高さ")]
    [SerializeField] Vector2 _bottomRig;
    /// <summary>LookAt対象</summary>
    Transform _lookTr;
    /// <summary>前フレームでのカメラの向き</summary>
    Vector3 _lastEulerAngles;
    /// <summary>マウスの入力値</summary>
    Vector2 _look;
    /// <summary>最後のhit</summary>
    RaycastHit _hit;
    [SerializeField] Transform GameObject;


    private void Start()
    {
        _vCam = GetComponent<CinemachineVirtualCameraBase>();
        if (_vCam.LookAt)
        {
            _lookTr = _vCam.LookAt;
        }
        else
        {
            _lookTr = transform;
            Debug.LogWarning($"{name}の{nameof(_vCam.LookAt)}がアサインされていません");
        }
    }


    /// <summary>マウス移動</summary>
    public void OnMouseLook(InputAction.CallbackContext context)
    {
        _look = context.ReadValue<Vector2>();
    }

    private void OnEnable()
    {

    }

    private void Update()
    {
        _topRig = _topRig.normalized;
        _middleRig = _middleRig.normalized;
        _bottomRig = _bottomRig.normalized;
        transform.root.position = _lookTr.position;
        transform.root.rotation = _lookTr.rotation;
        transform.Rotate(new Vector2(_look.y, _look.x) * new Vector2(_speed.x, _speed.y));
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        GameObject.eulerAngles = transform.eulerAngles;
        GameObject.position = _lookTr.position;
        Vector3 direction = transform.localPosition.normalized;
        Debug.Log($"{direction.x}, {Mathf.Atan2(_topRig.x, _topRig.y)}");
        if (direction.x > Mathf.Atan2(_topRig.y, _topRig.x)){

        }
        _lastEulerAngles = transform.eulerAngles;
    }

    private void LateUpdate()
    {
        _lastEulerAngles = transform.position - _lookTr.position;
    }
}
