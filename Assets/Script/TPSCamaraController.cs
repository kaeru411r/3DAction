using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

/// <summary>
/// TPSカメラの操作をするクラス
/// </summary>

[RequireComponent(typeof(CinemachineVirtualCamera), typeof(PlayerInput))]
public class TPSCamaraController : MonoBehaviour
{
    /// <summary>このスクリプトで操作をするVcam</summary>
    CinemachineVirtualCamera _vCam;
    [Tooltip("リグの最長半径")]
    [SerializeField] float _radius;
    [Tooltip("カメラ速度")]
    [SerializeField] Vector2 _speed;
    [Tooltip("仰角限界")]
    [SerializeField] float _elevationLimit;
    [Tooltip("俯角限界")]
    [SerializeField] float _depressionLimit;
    /// <summary>LookAt対象</summary>
    Transform _lookTr;
    /// <summary>マウスの入力値</summary>
    Vector2 _look;


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

        var angle = transform.localEulerAngles.x;
        //if (angle < _elevationLimit)
        //{
        //    Debug.Log($"1 {angle} {_elevationLimit}");
        //}
        //else if (angle > _depressionLimit)
        //{
        //    Debug.Log($"2 {angle} {_depressionLimit}");
        //}
        //else
        //{
            Debug.Log($"3 {angle} {_elevationLimit} {_depressionLimit}");
            Vector3 direction = transform.rotation * Vector3.forward * -1;
            Vector3 position = _lookTr.position;
            //transform.position = direction * radius + position;
            Debug.DrawRay(position, direction * _radius, Color.red);

            var t = _vCam.GetCinemachineComponent<CinemachineTransposer>();
            if (t)
            {
                t.m_FollowOffset = direction * _radius;
            }
        //}
    }

    public void SetPosition(Vector3 dir)
    {

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
            Gizmos.DrawWireSphere(_vCam.LookAt.position, _radius);
        }
    }


}
