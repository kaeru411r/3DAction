﻿using System.Collections;
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
    [Tooltip("マウスのカメラ速度")]
    [SerializeField] Vector2 _mouseSpeed;
    [Tooltip("パッドのカメラ速度")]
    [SerializeField] Vector2 _padSpeed;
    [Tooltip("上の限界点"), Range(-1, 1)]
    [SerializeField] float _upperLimit;
    [Tooltip("下の限界点"), Range(-1, 1)]
    [SerializeField] float _bottomLimit;

    /// <summary>LookAt対象</summary>
    Transform _lookTr;
    /// <summary>Follow対象</summary>
    Transform _followTr;
    /// <summary>マウスの入力値</summary>
    Vector2 _look;
    /// <summary>tureでマウス</summary>
    bool _isMouseorPad;
    /// <summary>vcamのtransposer</summary>
    CinemachineTransposer _transposer;
    /// <summary>ギズモの表示色</summary>
    Color _gizmosColor = new Color(1, 0, 0, 0.7f);


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
        if (_vCam.Follow)
        {
            _lookTr = _vCam.Follow;
        }
        else
        {
            _lookTr = transform;
            Debug.LogWarning($"{name}の{nameof(_vCam.Follow)}がアサインされていません");
        }
        _transposer = _vCam.GetCinemachineComponent<CinemachineTransposer>();
        if (!_transposer)
        {
            Debug.LogWarning($"{name}のBodyがTransposerに設定されていません");
        }
    }


    /// <summary>マウス移動</summary>
    //public void OnMouseLook(InputAction.CallbackContext context)
    //{
    //    _look = context.ReadValue<Vector2>();
    //    _isMouseorPad = true;
    //}

    ///// <summary>Pad移動</summary>
    //public void OnPadLook(InputAction.CallbackContext context)
    //{
    //    _look = context.ReadValue<Vector2>();
    //    _isMouseorPad = false;
    //}

    private void Update()
    {
        if (_isMouseorPad)
        {
            transform.Rotate(new Vector3(_look.y, _look.x) * _mouseSpeed);
        }
        else
        {
            transform.Rotate(new Vector3(_look.y, _look.x) * _padSpeed * Time.deltaTime);
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);

        float angle = transform.eulerAngles.x;
        Vector3 direction = transform.rotation * Vector3.forward * -1;
        {
            Vector3 position = _lookTr.position;
            Debug.DrawRay(position, direction * _radius, Color.red);
        }

        if (_transposer)
        {
            _transposer.m_FollowOffset = direction * _radius;
        }
        //float thetaU = 
        if (_transposer.m_FollowOffset.y > _radius * _upperLimit)
        {
            _look = new Vector2(_look.x, 0);
            var fO = _transposer.m_FollowOffset;
            float r = Mathf.Sqrt(_radius * _radius - (_radius * _upperLimit) * (_radius * _upperLimit));
            Vector2 xz = new Vector2(fO.x, fO.z);
            xz = xz.normalized * r;
            _transposer.m_FollowOffset = new Vector3(xz.x, _radius * _upperLimit, xz.y);
            transform.LookAt(_lookTr);
        }
        if (_transposer.m_FollowOffset.y < _radius * _bottomLimit)
        {
            var fO = _transposer.m_FollowOffset;
            float r = Mathf.Sqrt(_radius * _radius - (_radius * _bottomLimit) * (_radius * _bottomLimit));
            Vector2 xz = new Vector2(fO.x, fO.z);
            xz = xz.normalized * r;
            _transposer.m_FollowOffset = new Vector3(xz.x, _radius * _bottomLimit, xz.y);
            transform.LookAt(_lookTr);
        }
    }

    public void SetPosition(Vector3 dir)
    {

    }






    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _gizmosColor;
        if (!_vCam)
        {
            _vCam = GetComponent<CinemachineVirtualCamera>();
        }
        else if (_vCam.LookAt)
        {
            int segment = 72;
            float line = Mathf.PI * 2 / segment;
            Vector3 pos = transform.position + transform.up * -1 * -_bottomLimit * _radius;
            float radius = Mathf.Sqrt(_radius * _radius - _radius * _radius * _bottomLimit * _bottomLimit);
            for (float i = 0; i < Mathf.PI * 2; i += line)
            {
                Vector3 start = pos + radius * Mathf.Cos(i) * transform.forward + radius * Mathf.Sin(i) * transform.right;
                Vector3 goal = pos + radius * Mathf.Cos(i + line) * transform.forward + radius * Mathf.Sin(i + line) * transform.right;
                Gizmos.color = _gizmosColor;
                Gizmos.DrawLine(start, goal);
            }
            pos = transform.position + transform.up * -1 * -_upperLimit * _radius;
            radius = Mathf.Sqrt(_radius * _radius - _radius * _radius * _upperLimit * _upperLimit);
            for (float i = 0; i < Mathf.PI * 2; i += line)
            {
                Vector3 start = pos + radius * Mathf.Cos(i) * transform.forward + radius * Mathf.Sin(i) * transform.right;
                Vector3 goal = pos + radius * Mathf.Cos(i + line) * transform.forward + radius * Mathf.Sin(i + line) * transform.right;
                Gizmos.color = _gizmosColor;
                Gizmos.DrawLine(start, goal);
            }
            pos = transform.position;
            radius = _radius;
            for (float i = 0; i < Mathf.PI * 2; i += line)
            {
                Vector3 start = pos + radius * Mathf.Cos(i) * transform.up + radius * Mathf.Sin(i) * transform.forward;
                Vector3 goal = pos + radius * Mathf.Cos(i + line) * transform.up + radius * Mathf.Sin(i + line) * transform.forward;
                Gizmos.color = _gizmosColor;
                Gizmos.DrawLine(start, goal);
            }
        }
    }


}
