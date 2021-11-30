using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Text.RegularExpressions;
using Cinemachine;


/// <summary>
/// プレイヤー操作コンポーネント
/// 車両の操作を行う
/// </summary>
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
    [Tooltip("照準するレイヤー")]
    [SerializeField] LayerMask _layerMask;
    [Tooltip("FPS時の砲塔砲身の予約できる回転量の上限")]
    [SerializeField] Vector2 _maxDeltaRotation = new Vector2(10, 10);
    [Tooltip("マウス感度")]
    [SerializeField] Vector2 _mouseSensitivity;
    [Tooltip("TPSカメラ")]
    [SerializeField] CinemachineFreeLook _TPSCamara;
    [Tooltip("FPSカメラ")]
    [SerializeField] CinemachineVirtualCamera _FPSCamera;
    /// <summary>現在の視点</summary>
    [SerializeField] ViewMode _viewMode;
    /// <summary>移動用ベクトル</summary>
    Vector2 _move;
    /// <summary>視点操作用ベクトル</summary>
    Vector2 _look;
    /// <summary>現在入力しているデバイス</summary>
    InputDevice _inputDevice;


    private void Start()
    {
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
        _target = new GameObject().transform;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _move = context.ReadValue<Vector2>();
    }

    public void OnMouseLook(InputAction.CallbackContext context)
    {
        _inputDevice = InputDevice.Mouse;
        _look = context.ReadValue<Vector2>();
    }

    public void OnPadLook(InputAction.CallbackContext context)
    {
        _inputDevice = InputDevice.GamaPad;
        _look = context.ReadValue<Vector2>();
    }

    public void OnScroll(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _gunController?.Change(context.ReadValue<Vector2>().y);
        }
    }

    public void OnChange(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _gunController?.Change(1f);
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _gunController?.Fire(transform.root);
        }
    }

    public void OnChoice(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _gunController?.Choice(int.Parse(Regex.Replace(context.control.ToString(), @"[^0-9]", "")));
        }
    }


    // Update is called once per frame
    void Update()
    {
        _characterBase?.Move(_move);
        _TPSCamara.m_XAxis.m_MaxSpeed = _mouseSensitivity.x * 20;
        _TPSCamara.m_YAxis.m_MaxSpeed = _mouseSensitivity.y / 2;

        if (_gunController)
        {
            if (_viewMode == ViewMode.TPS)
            {
                TPSAim();

            }
            else
            {
                if (_inputDevice == InputDevice.Mouse)
                {
                    MouseFPSAim();
                }
                else if (_inputDevice == InputDevice.GamaPad)
                {
                    PadTPSAim();
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
        Vector2 dif = _sight.eulerAngles - new Vector3(barrel.x, turret.y);
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
        if (dif.y > _maxDeltaRotation.x)
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
        if (dif.x > _maxDeltaRotation.y)
        {
            _sight.Rotate(_maxDeltaRotation.y - dif.x, 0, 0);
        }
        else if (dif.x < -_maxDeltaRotation.y)
        {
            _sight.Rotate(-_maxDeltaRotation.y - dif.x, 0, 0);
        }
    }


    /// <summary>ゲームパッドでのFPS操作</summary>
    void PadTPSAim()
    {
        Vector2 gunSpeed = _gunController.GunMoveSpeed;
        Vector3 barrel = _gunController.Barrel;
        Vector3 turret = _gunController.Turret;
        _sight.eulerAngles = new Vector3(barrel.x + gunSpeed.y * _look.y, turret.y + gunSpeed.x * _look.x);
    }

    /// <summary>
    /// 視点モード
    /// </summary>
    enum ViewMode
    {
        /// <summary>一人称視点</summary>
        FPS,
        /// <summary>一人称視点でズーム</summary>
        Zoom,
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
