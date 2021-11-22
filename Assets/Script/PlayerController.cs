using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Text.RegularExpressions;


/// <summary>
/// プレイヤー操作コンポーネント
/// 車両の操作を行う
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] GunController _gunController;
    [SerializeField] CharacterBase _characterBase;
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
    [SerializeField] float _maxDeltaRotation;
    ViewMode _viewMode = ViewMode.TPS;
    Vector2 _move;
    Vector2 _look;

    private void Start()
    {
        _target = new GameObject().transform;
        _sight = _gunController.Sight;
        if(_maxDeltaRotation < 10)
        {
            _maxDeltaRotation = 10;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //Debug.Log(context.ReadValue<Vector2>());
        _look = context.ReadValue<Vector2>();
    }

    public void OnScroll(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _gunController.Change(context.ReadValue<Vector2>().y);
        }
    }

    public void OnChange(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            _gunController.Change(1f);
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            _gunController.Fire(transform.root);
        }
    }

    public void OnChoice(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _gunController.Choice(int.Parse(Regex.Replace(context.control.ToString(), @"[^0-9]", "")));
        }
    }


    // Update is called once per frame
    void Update()
    {
        _characterBase.Move(_move);



        if (_viewMode == ViewMode.TPS)
        {
            TPSAim();
        }
        else
        {
            FPSAim();
        }
    }

    void TPSAim()
    {
        float centerX = Screen.width / 2;
        float centerY = Screen.height / 2;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(_crosshair.rectTransform.position);
        if (Physics.Raycast(ray, out hit, _distance, _layerMask)){
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

    void FPSAim()
    {
        _sight.Rotate(_look.y, _look.x, 0);
        Debug.Log($"#1 {_sight.eulerAngles.y - _look.y} + {_look.y} = {_sight.eulerAngles.y}");
        Vector3 sight = _sight.rotation.eulerAngles;
        Vector3 camera = Camera.main.transform.rotation.eulerAngles;
        Debug.Log($"#2 {sight.y} - {camera.y} = {sight.y - camera.y}");
        if (sight.x - camera.x > _maxDeltaRotation)
        {
            _sight.eulerAngles = new Vector3(camera.x + _maxDeltaRotation, 0, 0);
            Debug.Log(1);
        }
        else if (sight.x - camera.x < -_maxDeltaRotation)
        {
            _sight.eulerAngles = new Vector3(camera.x - _maxDeltaRotation, 0, 0);
            Debug.Log(2);
        }
        if (sight.y - camera.y > _maxDeltaRotation)
        {
            //Debug.Log($"{sight.y - camera.y} {_maxDeltaRotation} 1");
            _sight.eulerAngles = new Vector3(0, camera.y + _maxDeltaRotation, 0);
            Debug.Log(3);
        }
        else if (sight.y - camera.y < -_maxDeltaRotation)
        {
            //Debug.Log($"{sight.y - camera.y} {_maxDeltaRotation} 2");
            _sight.eulerAngles = new Vector3(0, camera.y - _maxDeltaRotation, 0);
            Debug.Log(4);
        }
    }

    /// <summary>
    /// 視点モード
    /// </summary>
    enum ViewMode
    {
        FPS,
        Zoom,
        TPS,
    }

}
