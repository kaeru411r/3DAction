using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

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

    /// <summary>Pad移動</summary>
    public void OnPadLook(Vector2 look)
    {
        _look = look;
        _isMouseorPad = false;
    }

    private void Update()
    {
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

    }

    /// <summary>
    /// カメラの位置調整
    /// </summary>
    void SetPosition()
    {
        //カメラの向きの単位ベクトル
        Vector3 direction = transform.rotation * Vector3.forward * -1;

        //デバッグ用可視線照射
        Vector3 position = _lookTr.position;
        Debug.DrawRay(position, direction * _radius, Color.red);

        //カメラの向きにあわせて位置を調整
        _transposer.m_FollowOffset = direction * _radius;

        _mark.position = transform.position;
        float bottom = Mathf.Min(_limit0, _limit1) * _radius;
        float top = Mathf.Max(_limit0, _limit1) * _radius;

        if (top < _mark.localPosition.y)            //カメラが範囲より上に出ていた時
        {
            Debug.Log(1);
        }
        else if (bottom > _mark.localPosition.y)    //カメラが範囲より下に出ていた時
        {
            Debug.Log(2);
        }

    }

    /// <summary>
    /// カメラの位置が範囲内に収まっているかチェックする
    /// </summary>
    void PositionCheck()
    {

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
