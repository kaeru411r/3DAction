﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Text.RegularExpressions;
using Cinemachine;
using System;


/// <summary>
/// プレイヤー操作コンポーネント
/// プレイヤーによる車両の操作を行う
/// </summary>
[RequireComponent(typeof(GunController), typeof(CharacterBase))]
public class PlayerController : MonoBehaviour
{
    GunController _gunController;
    CharacterBase _characterBase;
    /// <summary>照準先</summary>
    Transform _target;
    /// <summary>サイトオブジェクトのトランスフォーム</summary>
    Transform _sight;
    [Tooltip("レティクル")]
    [SerializeField] Image _crosshair;
    [Tooltip("rayを飛ばす距離")]
    [SerializeField] float _distance;
    [Tooltip("フィールド上で見える自分以外のレイヤー")]
    [SerializeField] LayerMask _layerMask;
    [Tooltip("FPS時の砲塔砲身の予約できる回転量の上限")]
    [SerializeField] Vector2 _maxDeltaRotation = new Vector2(10, 10);
    [Tooltip("マウス感度")]
    [SerializeField] Vector2 _mouseSensitivity;
    [Tooltip("TPSカメラ")]
    [SerializeField] CinemachineFreeLook _tpsVCam;
    [Tooltip("TPSカメラに近い位置に設置した中間VCam")]
    [SerializeField] CinemachineVirtualCameraBase _intermediateVCam;
    [Tooltip("FPSカメラ")]
    [SerializeField] CinemachineVirtualCamera _fpsVCam;
    [Tooltip("TPSのデフォルトの視野角")]
    [SerializeField] float _tpsFov;
    [Tooltip("望遠鏡の倍率")]
    [SerializeField] float _scopeMagnification;
    /// <summary>現在の視点</summary>
    [SerializeField] ViewMode _viewMode;
    /// <summary>移動用ベクトル</summary>
    Vector2 _move;
    /// <summary>視点操作用ベクトル</summary>
    Vector2 _look;
    /// <summary>現在入力しているデバイス</summary>
    InputDevice _inputDevice;
    /// <summary>デフォルトのLayerMask</summary>
    LayerMask _defaltLayerMask;
    /// <summary>ズームしているか否か</summary>
    bool _isZoom = false;
    /// <summary>FPSのデフォルトの視野角</summary>
    float _fpsFov;
    /// <summary>_target用オブジェクトの名前</summary>
    string _targetName = "TPSTarget";

    //private void OnEnable()
    //{
    //    Cursor.visible = false;
    //}

    //private void OnDisable()
    //{
    //    Cursor.visible = true;
    //}

    private void Start()
    {
        _fpsFov = _fpsVCam.m_Lens.FieldOfView;
        _tpsVCam.m_Lens.FieldOfView = _tpsFov;
        _defaltLayerMask = Camera.main.cullingMask;
        _gunController = GetComponent<GunController>();
        if (!_gunController)
        {
            Debug.LogError($"{name}にGunControllerコンポーネントが見つかりませんでした");
        }
        else
        {
            _sight = _gunController.Sight;
        }
        _characterBase = GetComponent<CharacterBase>();
        if (!_characterBase)
        {
            Debug.LogError($"{name}にCharacterBaseコンポーネントが見つかりませんでした");
        }
        SetTarget();
        if (_viewMode == ViewMode.FPS)
        {
            _fpsVCam.MoveToTopOfPrioritySubqueue();
        }
        if (_viewMode == ViewMode.TPS)
        {
            StartCoroutine(TPSSetUp());
        }
    }

    /// <summary>_targetの用意</summary>
    void SetTarget()
    {
        var go = new GameObject();
        go.name = _targetName;
        _target = go.transform;
    }

    /// <summary>TPSからスタートする際、変な方向を向いているTPSカメラの向きを正す</summary>
    IEnumerator TPSSetUp()
    {
        _fpsVCam.MoveToTopOfPrioritySubqueue();
        yield return null;
        _intermediateVCam.MoveToTopOfPrioritySubqueue();
        yield return null;
        _tpsVCam.MoveToTopOfPrioritySubqueue();
    }

    /// <summary>WASD及び左スティック</summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        _move = context.ReadValue<Vector2>();
    }
    /// <summary>マウス移動</summary>
    public void OnMouseLook(InputAction.CallbackContext context)
    {
        _inputDevice = InputDevice.Mouse;
        _look = context.ReadValue<Vector2>();
    }
    /// <summary>右スティック</summary>
    public void OnPadLook(InputAction.CallbackContext context)
    {
        _inputDevice = InputDevice.GamaPad;
        _look = context.ReadValue<Vector2>();
    }
    /// <summary>マウスホイール</summary>
    public void OnScroll(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _gunController?.Change(context.ReadValue<Vector2>().y);
        }
    }
    /// <summary>右側上ボタン</summary>
    public void OnChange(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _gunController?.Change(1f);
        }
    }
    /// <summary>左クリック及び右トリガー</summary>
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _gunController?.Fire();
        }
    }
    /// <summary>数字キー</summary>
    public void OnNumber(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _gunController?.Choice(int.Parse(Regex.Replace(context.control.ToString(), @"[^0-9]", "")));
        }
    }
    /// <summary>cキー及び右側右ボタン</summary>
    public void OnCamera(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            CameraChange();
        }
    }
    /// <summary>右クリック及び左トリガー</summary>
    public void OnAim(InputAction.CallbackContext context)
    {
        
        if (context.ReadValue<float>() != 0)
        {
            _isZoom = true;
        }
        else
        {
            _isZoom = false;
        }
    }


    // Update is called once per frame
    void Update()
    {
        _characterBase?.Move(_move);
        _tpsVCam.m_XAxis.m_MaxSpeed = _mouseSensitivity.x * 20;
        _tpsVCam.m_YAxis.m_MaxSpeed = _mouseSensitivity.y / 2;

        if (_gunController)
        {
            if (_viewMode == ViewMode.TPS)
            {
                TPSAim();
                Camera.main.cullingMask = _defaltLayerMask;
            }
            else
            {
                FPSZoom();
                if (_inputDevice == InputDevice.Mouse)
                {
                    MouseFPSAim();
                }
                else if (_inputDevice == InputDevice.GamaPad)
                {
                    PadFPSAim();
                }
            }
        }
    }

    /// <summary>TPS時の視点操作</summary>
    void TPSAim()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(_crosshair.rectTransform.position);
        if (Physics.Raycast(ray, out hit, _distance, _layerMask))
        {
            _target.position = hit.point;
        }
        else
        {
            Vector3 direction = ray.direction;
            Vector3 position = ray.origin;
            _target.position = direction * _distance + position;
        }
        _sight.LookAt(_target.position);
    }

    /// <summary>マウスでのFPS操作</summary>
    void MouseFPSAim()
    {
        Vector3 barrel = _gunController.Barrel;
        Vector3 turret = _gunController.Turret;
        Vector2 dif = _sight.localEulerAngles - new Vector3(barrel.x, turret.y);
        _sight.Rotate(_look.y * _mouseSensitivity.y, _look.x * _mouseSensitivity.x, 0);

        //  ヨー制御
        if (dif.y < -180)
        {
            dif = new Vector3(dif.x, dif.y + 360);
        }
        else if (dif.y > 180)
        {
            dif = new Vector3(dif.x, dif.y - 360);
        }
        if (dif.y > _maxDeltaRotation.x)    //  sightが大きく動き過ぎないよう制御
        {
            _sight.Rotate(0, _maxDeltaRotation.x - dif.y, 0);
        }
        else if (dif.y < -_maxDeltaRotation.x)
        {
            _sight.Rotate(0, -_maxDeltaRotation.x - dif.y, 0);
        }

        //  ピッチ制御
        if (dif.x < -180)
        {
            dif = new Vector3(dif.x + 360, dif.y);
        }
        else if (dif.x > 180)
        {
            dif = new Vector3(dif.x - 360, dif.y);
        }
        if (dif.x > _maxDeltaRotation.y)    //  sightが大きく動き過ぎないよう制御
        {
            _sight.Rotate(_maxDeltaRotation.y - dif.x, 0, 0);
        }
        else if (dif.x < -_maxDeltaRotation.y)
        {
            _sight.Rotate(-_maxDeltaRotation.y - dif.x, 0, 0);
        }
    }

    /// <summary>ゲームパッドでのFPS操作</summary>
    void PadFPSAim()
    {
        Vector2 gunSpeed = _gunController.GunMoveSpeed;
        Vector3 barrel = _gunController.Barrel;
        Vector3 turret = _gunController.Turret;
        _sight.localEulerAngles = new Vector3(barrel.x + gunSpeed.y * _look.y, turret.y + gunSpeed.x * _look.x);
    }

    /// <summary>FPS時のズーム切り替え</summary>
    void FPSZoom()
    {
        if (_isZoom)
        {
            float fovrad = (float)(2 * Math.Atan((1 / _scopeMagnification) * Math.Tan(_fpsFov * Math.PI / 360)));
            _fpsVCam.m_Lens.FieldOfView = (float)(fovrad * 180 / Math.PI);
            Camera.main.cullingMask = _layerMask;
        }
        else
        {
            _fpsVCam.m_Lens.FieldOfView = _fpsFov;
            Camera.main.cullingMask = _defaltLayerMask;
        }
    }

    /// <summary>カメラの切り替えを行う</summary>
    void CameraChange()
    {
        if (_viewMode == ViewMode.FPS)
        {
            _viewMode = ViewMode.TPS;
            StartCoroutine(FPSToTPS());

        }
        else if (_viewMode == ViewMode.TPS)
        {
            _viewMode = ViewMode.FPS;
            _fpsVCam.MoveToTopOfPrioritySubqueue();
        }
    }

    /// <summary>FPSからTPSに変える際、TPSカメラの位置をより正しい位置に動かす</summary>
    /// <returns></returns>
    IEnumerator FPSToTPS()
    {
        _intermediateVCam.MoveToTopOfPrioritySubqueue();
        yield return null;
        _tpsVCam.MoveToTopOfPrioritySubqueue();
    }



    /// <summary>
    /// 視点モード
    /// </summary>
    enum ViewMode
    {
        /// <summary>一人称視点</summary>
        FPS,
        /// <summary>三人称視点</summary>
        TPS,
    }

    /// <summary>
    /// 入力デバイス
    /// </summary>
    enum InputDevice
    {
        /// <summary>マウス</summary>
        Mouse,
        /// <summary>ゲームパッド</summary>
        GamaPad,
    }

}
