using System.Collections;
using System.Collections.Generic;
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
public class PlayerTurret : MonoBehaviour
{
    [Tooltip("ターレット")]
    [SerializeField] Turret _turret;
    [Tooltip("この車両の" + nameof(CharacterBase))]
    [SerializeField] CharacterBase _characterBase;
    [Tooltip("レティクル")]
    [SerializeField] Image _crosshair;
    [Tooltip("rayを飛ばす距離")]
    [SerializeField] float _distance = 100;
    [Tooltip("フィールド上で見える自分以外のレイヤー")]
    [SerializeField] LayerMask _layerMask;
    [Tooltip("FPS時の砲塔砲身の予約できる回転量の上限")]
    [SerializeField] Vector2 _maxDeltaRotation = new Vector2(10, 10);
    [Tooltip("マウス感度")]
    [SerializeField] Vector2 _mouseSensitivity = Vector2.one * 10;
    [Tooltip("TPSカメラ")]
    [SerializeField] CinemachineVirtualCamera _tpsVCam;
    [Tooltip("TPSカメラに近い位置に設置した中間VCam")]
    [SerializeField] CinemachineVirtualCameraBase _intermediateVCam;
    [Tooltip("FPSカメラ")]
    [SerializeField] CinemachineVirtualCamera _fpsVCam;
    [Tooltip("TPSのデフォルトの視野角")]
    [SerializeField] float _tpsFov = 70;
    [Tooltip("望遠鏡の倍率")]
    [SerializeField] float _scopeMagnification = 2;
    [Tooltip("TPSカメラの参照トランスフォーム")]
    [SerializeField] Transform _tpsCamBass;
    [SerializeField] Text _hpText;
    [Tooltip("現在の視点")]
    [SerializeField] ViewMode _viewMode;
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
    /// <summary>砲弾の集約</summary>
    GameObjectCollector _collector;

    public CharacterBase Charactor { get { return _characterBase; } }


    private void OnEnable()
    {
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Cursor.visible = true;
    }

    private void OnDestroy()
    {
        Destroy(transform.root.gameObject);
    }

    private void Start()
    {
        _collector = new GameObjectCollector(name);
        _fpsFov = _fpsVCam.m_Lens.FieldOfView;
        _tpsVCam.m_Lens.FieldOfView = _tpsFov;
        _defaltLayerMask = Camera.main.cullingMask;
        if (!_turret)
        {
            Debug.LogWarning($"{nameof(_turret)}が設定されていません");
        }
        if (_viewMode == ViewMode.FPS)
        {
            _fpsVCam.MoveToTopOfPrioritySubqueue();
        }
        if (_viewMode == ViewMode.TPS)
        {
            //StartCoroutine(TPSSetUp());
            _tpsVCam.MoveToTopOfPrioritySubqueue();
        }
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

    #region 入力受付部
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
            //_turret?.Change(context.ReadValue<Vector2>().y);
        }
    }
    /// <summary>右側上ボタン</summary>
    public void OnChange(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //_turret?.Change(1f);
        }
    }
    /// <summary>左クリック及び右トリガー</summary>
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_turret)
            {
                List<BaseBullet> bullets = _turret?.Fire();
                //List<Bullet> bullets = Call(_turret.Fire);
                foreach (BaseBullet bullet in bullets)
                {
                    _collector.Collection(bullet);
                }
            }
        }
    }
    /// <summary>数字キー</summary>
    public void OnNumber(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //_turret?.Choice(int.Parse(Regex.Replace(context.control.ToString(), @"[^0-9]", "")));
        }
    }
    /// <summary>cキー及び右側右ボタン</summary>
    public void OnCamera(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //CameraChange();
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

    #endregion


    // Update is called once per frame
    void Update()
    {
        if (_hpText && _characterBase)
        {
            _hpText.text = $"HP {_characterBase.Hp}";
        }

        if (_turret)
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

        Vector3 point = Physics.Raycast(ray, out hit, _distance, _layerMask) ? hit.point : (ray.direction * _distance + ray.origin);

        _turret.Sight.LookAt(point);
    }

    /// <summary>マウスでのFPS操作</summary>
    void MouseFPSAim()
    {
        Vector3 barrel = _turret.Barrel.localEulerAngles;
        Vector3 turret = _turret.transform.localEulerAngles;
        Vector2 dif = _turret.Sight.localEulerAngles - new Vector3(barrel.x, turret.y);
        _turret.Sight.Rotate(_look.y * _mouseSensitivity.y, _look.x * _mouseSensitivity.x, 0);

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
            _turret.Sight.Rotate(0, _maxDeltaRotation.x - dif.y, 0);
        }
        else if (dif.y < -_maxDeltaRotation.x)
        {
            _turret.Sight.Rotate(0, -_maxDeltaRotation.x - dif.y, 0);
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
            _turret.Sight.Rotate(_maxDeltaRotation.y - dif.x, 0, 0);
        }
        else if (dif.x < -_maxDeltaRotation.y)
        {
            _turret.Sight.Rotate(-_maxDeltaRotation.y - dif.x, 0, 0);
        }
    }

    /// <summary>ゲームパッドでのFPS操作</summary>
    void PadFPSAim()
    {
        Vector2 gunSpeed = _turret.GunMoveSpeed * Time.deltaTime;
        Vector3 barrel = _turret.Barrel.localEulerAngles;
        Vector3 turret = _turret.transform.localEulerAngles;
        _turret.Sight.localEulerAngles = new Vector3(barrel.x + gunSpeed.y * _look.y, turret.y + gunSpeed.x * _look.x);
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
