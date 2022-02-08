using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

/// <summary>
/// TPSカメラの操作をするクラス
/// </summary>

[RequireComponent (typeof(CinemachineVirtualCamera))]
public class TPSCamaraController : MonoBehaviour
{
    /// <summary>このスクリプトで操作をするVcam</summary>
    CinemachineVirtualCamera _vCam;
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
    /// <summary>最後のhit</summary>
    RaycastHit _hit;
    [SerializeField] Transform GameObject;


    private void Start()
    {
        _vCam = GetComponent<CinemachineVirtualCamera>();
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
        //transform.root.position = _lookTr.position;
        //transform.root.rotation = _lookTr.rotation;
        transform.Rotate(new Vector3(_look.y, _look.x) * _speed);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        if (GameObject)
        {
            GameObject.eulerAngles = transform.eulerAngles;
            GameObject.position = _lookTr.position;
        }
        float radius = 5;


        Vector3 direction = Quaternion.Euler(transform.eulerAngles) * Vector3.forward * -1;
        Vector3 position = _lookTr.position;
        //transform.position = direction * radius + position;
        Debug.DrawRay(position, direction * radius, Color.red);

        var t = _vCam.GetCinemachineComponent<CinemachineTransposer>();
        if (t)
        {
            t.m_FollowOffset = direction * radius;
        }

        //Physics.queriesHitBackfaces = true;
        //Debug.DrawRay(_lookTr.position, transform.forward * -1 * _maxRadius, Color.red);
        //if (Physics.Raycast(_lookTr.position, transform.forward * -1, out _hit, _maxRadius, _layerMask))
        //{
        //    transform.position = _hit.point;
        //    Debug.Log(1);
        //}
        //else
        //{

        //}
        //Physics.queriesHitBackfaces = false;
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
            _vCam = GetComponent<CinemachineVirtualCamera>();
        }
        else if (_vCam.LookAt)
        {
            Gizmos.DrawWireSphere(_vCam.LookAt.position, _maxRadius);
        }
    }

}
