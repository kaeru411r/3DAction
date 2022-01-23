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
    [Tooltip("リグのレイヤーマスク")]
    [SerializeField] LayerMask _layerMask;
    [Tooltip("カメラ速度")]
    [SerializeField] Vector2 _speed;
    /// <summary>LookAt対象</summary>
    Transform _lookTr;
    /// <summary>前フレームでのカメラの向き</summary>
    Vector3 _lastEulerAngles;
    /// <summary>マウスの入力値</summary>
    Vector2 _look;


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
        //transform.Rotate(new Vector3(_look.y, _look.x) * new Vector2(_speed.x, _speed.y));
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);

        RaycastHit hit;

        if (Physics.Raycast(_lookTr.position, (transform.forward * -1).normalized, out hit, _maxRadius, _layerMask))
        {
            transform.position = hit.point;
            Debug.Log(1);
        }
        else
        {

        }
        _lastEulerAngles = transform.eulerAngles;
    }

    private void LateUpdate()
    {
        _lastEulerAngles = transform.position - _lookTr.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0, 0, 1f);
        if (!_vCam)
        {
            _vCam = GetComponent<CinemachineVirtualCameraBase>();
        }

        if (_vCam.LookAt)
        {
            Gizmos.DrawWireSphere(_vCam.LookAt.position, _maxRadius);
        }
    }

}
