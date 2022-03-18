﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// TPSカメラの操作をするクラス
/// </summary>

[RequireComponent(typeof(CinemachineVirtualCamera))]
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
    [SerializeField] float _limit0;
    [Tooltip("下の限界点"), Range(-1, 1)]
    [SerializeField] float _limit1;
    [SerializeField] Transform _testMark;
    [SerializeField, Range(-1, 1)] float _x;
    [SerializeField, Range(-1, 1)] float _y;


    /// <summary>カメラの座標のフォローオブジェクトのローカル版</summary>
    Transform _mark;
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
            _followTr = _vCam.Follow;
            _mark = Instantiate(new GameObject(), _followTr).transform;
            _mark.name = nameof(_mark);
        }
        else
        {
            _followTr = transform;
            Debug.LogWarning($"{name}の{nameof(_vCam.Follow)}がアサインされていません");
        }
        _vCam.DestroyCinemachineComponent<CinemachineOrbitalTransposer>();
        _transposer = _vCam.GetCinemachineComponent<CinemachineTransposer>();
        if (!_transposer)
        {
            _transposer = _vCam.AddCinemachineComponent<CinemachineTransposer>();
        }
        _transposer.m_BindingMode = CinemachineTransposer.BindingMode.WorldSpace;
        _transposer.m_XDamping = 0;
        _transposer.m_YDamping = 0;
        _transposer.m_ZDamping = 0;
    }


    /// <summary>マウス移動</summary>
    public void OnMouseLook(Vector2 look)
    {
        _look = look;
        _isMouseorPad = true;
    }
    /// <summary>マウス移動</summary>
    public void OnMouseLook(float x, float y)
    {
        _look = new Vector2(x, y);
        _isMouseorPad = true;
    }

    /// <summary>Pad移動</summary>
    public void OnPadLook(Vector2 look)
    {
        _look = look;
        _isMouseorPad = false;
    }
    /// <summary>Pad移動</summary>
    public void OnPadLook(float x, float y)
    {
        _look = new Vector2(x, y);
        _isMouseorPad = false;
    }

    private void Update()
    {
        OnPadLook(_x, _y);
        //Debug.Log($"{_testMark.position}, {transform.position}U1");
        //マウスとパッドでそれぞれカメラ旋回
        if (_isMouseorPad)
        {
            transform.Rotate(new Vector3(_look.y, _look.x) * _mouseSpeed);
        }
        else
        {
            transform.Rotate(new Vector3(_look.y, _look.x) * _padSpeed * Time.deltaTime);
        }

        //ロールを0に
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);

        SetPosition();
        //_mark.position = transform.position;
        //_testMark.position = transform.position;
        //Debug.Log($"{_testMark.position}, {transform.position}U2");
        //if(transform.position != new Vector3(0, 0, -10) && p >= 20)
        //{
        //    UnityEditor.EditorApplication.isPaused = true;
        //    Debug.Log($"{transform.position != new Vector3(0, 0, -10)}");
        //}
        //else
        //{
        //    Debug.Log($"{transform.position != new Vector3(0, 0, -10)}");
        //}
        //p++;
        //Debug.Log(p);

    }
    int p = 0; 

    private void LateUpdate()
    {
        //Debug.Log($"{_mark.position}, {transform.position}L");
    }

    /// <summary>
    /// カメラの位置調整
    /// </summary>
    void SetPosition()
    {
        //カメラの向きの単位ベクトル
        Vector3 direction = transform.rotation * Vector3.back;

        //デバッグ用可視線照射
        Vector3 position = _lookTr.position;
        Debug.DrawRay(position, direction * _radius, Color.red);

        //カメラの向きにあわせて位置を調整
        _transposer.m_FollowOffset = direction * _radius;
        transform.position = _followTr.position + _transposer.m_FollowOffset;
        //transform.LookAt(_followTr);

        _mark.position = transform.position;
        _testMark.position = transform.position;
        //Debug.Log($"{_mark.position}, {transform.position}");
        //Debug.Log($"{_mark.position == transform.position}");
        //if (_mark.position != transform.position)
        //{
        //    Debug.Log($"{_mark.position == transform.position}");
        //}

        //Debug.Log($"{top}, {_mark.localPosition.y}");
        if (_limit0 * _radius < _mark.localPosition.y)            //カメラが範囲より上に出ていた時
        {
            PositionCorrection(direction, _limit0);
        }
        else if (_limit1 * _radius > _mark.localPosition.y)    //カメラが範囲より下に出ていた時
        {
            PositionCorrection(direction, _limit1);
        }

    }

    /// <summary>
    /// カメラの位置が範囲内に収まっているかチェックする
    /// </summary>
    void PositionCorrection(Vector3 direction, float limit)
    {

        //Followを中心とし、カメラ上を通る円の回転軸の方向ベクトル
        Vector3 v0 = Quaternion.Euler(0, 90, 0) * direction;
        v0 = new Vector3(v0.x, 0, v0.z).normalized;
        Debug.DrawRay(_followTr.position, v0);
        float d0 = v0.x * _followTr.position.x + v0.y * _followTr.position.y + v0.z * _followTr.position.z;
        //制限の円の回転軸の方向ベクトル
        Vector3 v1 = _followTr.up;
        Vector3 cPos = _followTr.position + _followTr.up * -1 * -limit * _radius;
        Debug.DrawRay(cPos, v1);
        float d1 = v1.x * cPos.x + v1.y * cPos.y + v1.z * cPos.z;
        //各円を含む二つの面の接線の方向ベクトル
        Vector3 e = new Vector3(v0.y * v1.z - v0.z * v1.y, v0.z * v1.x - v0.x * v1.z, v0.x * v1.y - v0.y * v1.x).normalized;

        Vector3 a = Vector3.zero;

        if (e.z != 0)
        {
            a = new Vector3((d0 * v1.y - d1 * v0.y) / e.z, (d0 * v1.x - d1 * v0.x ) / (-e.z), 0);
            Debug.Log($"1 {e}");
        }
        else if (e.y != 0)
        {
            a = new Vector3((d0 * v1.z - d1 * v0.z) / (-e.y), 0, (d0 * v1.x - d1 * v0.x) / e.y);
            Debug.Log($"2 {e}");
        }
        else if (e.x != 0)
        {
            a = new Vector3(0, (d0 * v1.z - d1 * v0.z) / e.x, (d0 * v1.y - d1 * v0.y ) / (-e.x));
            Debug.Log($"3 {e}");
        }

        Debug.DrawRay(a, e * 100);
        Debug.DrawRay(a, e * -100);
        //線の本数
        int segment = 72;
        //線一本あたりの角度
        float theta = Mathf.PI * 2 / segment;;
        for (float i = 0; i < Mathf.PI * 2; i += theta)
        {
            Vector3 start = _followTr.position + _radius * Mathf.Cos(i) * direction + _radius * Mathf.Sin(i) * (Quaternion.Euler(90, 0, 0) * direction);
            Vector3 goal = _followTr.position + _radius * Mathf.Cos(i + theta) * direction + _radius * Mathf.Sin(i + theta) * (Quaternion.Euler(90, 0, 0) * direction);
            Debug.DrawLine(start, goal, _gizmosColor);
        }

        Vector3 buf = (a - _followTr.position);
        float d = Vector3.Dot(e, buf) * Vector3.Dot(e, a + _followTr.position) - (Vector3.Dot(a, a) - _radius * _radius);

        Vector3 pos;

        if (d > 0)
        {
            float t0 = -Vector3.Dot(e, buf) + Mathf.Sqrt(d);
            float t1 = -Vector3.Dot(e, buf) - Mathf.Sqrt(d);
            float t = Vector3.Distance(a + e * t0, transform.position) < Vector3.Distance(a + e * t1, transform.position) ? t0 : t1;
            pos = a + e * t;
        }
        else if (d == 0)
        {
            float t = -Vector3.Dot(e, buf);
            pos = a + e * t;
        }
        else
        {
            pos = transform.position;
        }
        _testMark.position = pos;
        _transposer.m_FollowOffset = pos - _followTr.position;
        transform.position = _followTr.position + _transposer.m_FollowOffset;
        transform.LookAt(_followTr);
        //Debug.Log($"{_testMark.position == transform.position}");
        UnityEditor.EditorApplication.isPaused = true;
    }




    /// <summary>
    /// カメラのリグの表示
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _gizmosColor;
        if (!_vCam)
        {
            _vCam = GetComponent<CinemachineVirtualCamera>();
        }
        else if (_vCam.Follow)
        {
            if (!_followTr)
            {
                _followTr = _vCam.Follow;
            }

            //線の本数
            int segment = 72;
            //線一本あたりの角度
            float theta = Mathf.PI * 2 / segment;
            //円の中心の座標
            Vector3 pos = _followTr.position + _followTr.up * -1 * -_limit1 * _radius;
            //円の半径
            float radius = Mathf.Sqrt(_radius * _radius - _radius * _radius * _limit1 * _limit1);
            for (float i = 0; i < Mathf.PI * 2; i += theta)
            {
                Vector3 start = pos + radius * Mathf.Cos(i) * _followTr.forward + radius * Mathf.Sin(i) * _followTr.right;
                Vector3 goal = pos + radius * Mathf.Cos(i + theta) * _followTr.forward + radius * Mathf.Sin(i + theta) * _followTr.right;
                Gizmos.color = _gizmosColor;
                Gizmos.DrawLine(start, goal);
            }

            pos = _followTr.position + _followTr.up * -1 * -_limit0 * _radius;
            radius = Mathf.Sqrt(_radius * _radius - _radius * _radius * _limit0 * _limit0);
            for (float i = 0; i < Mathf.PI * 2; i += theta)
            {
                Vector3 start = pos + radius * Mathf.Cos(i) * _followTr.forward + radius * Mathf.Sin(i) * _followTr.right;
                Vector3 goal = pos + radius * Mathf.Cos(i + theta) * _followTr.forward + radius * Mathf.Sin(i + theta) * _followTr.right;
                Gizmos.color = _gizmosColor;
                Gizmos.DrawLine(start, goal);
            }

            pos = _followTr.position;
            radius = _radius;
            for (float i = 0; i < Mathf.PI * 2; i += theta)
            {
                Vector3 start = pos + radius * Mathf.Cos(i) * _followTr.up + radius * Mathf.Sin(i) * _followTr.forward;
                Vector3 goal = pos + radius * Mathf.Cos(i + theta) * _followTr.up + radius * Mathf.Sin(i + theta) * _followTr.forward;
                Gizmos.color = _gizmosColor;
                Gizmos.DrawLine(start, goal);
            }
        }
    }
}
