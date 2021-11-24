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
    /// <summary>バレルオブジェクトのトランスフォーム</summary>
    Transform _barrel;
    [Tooltip("レティクル")]
    [SerializeField] Image _crosshair;
    [Tooltip("rayを飛ばす距離")]
    [SerializeField] float _distance;
    [Tooltip("照準するレイヤー")]
    [SerializeField] LayerMask _layerMask;
    [Tooltip("FPS時の砲塔砲身の予約できる回転量の上限")]
    [SerializeField] Vector2 _maxDeltaRotation;
    /// <summary>現在の視点</summary>
    [SerializeField] ViewMode _viewMode;
    /// <summary>移動用ベクトル</summary>
    Vector2 _move;
    /// <summary>視点操作用ベクトル</summary>
    Vector2 _look;
    

    private void Start()
    {
        _target = new GameObject().transform;
        _sight = _gunController.Sight;
        _barrel = _gunController.Barrel;
        if(_maxDeltaRotation == Vector2.zero)
        {
            Debug.LogError("_maxDeltaRotationを設定しろ");
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
        _look = new Vector2(0, 1);
        _look = _look.normalized;
        Vector3 dif = _sight.eulerAngles - Camera.main.transform.eulerAngles;
        _sight.Rotate(_look.y, _look.x, 0);

        ////  ヨー制御
        //if (dif.y < -180)
        //{
        //    dif = new Vector3(dif.x, dif.y + 360, dif.z);
        //}
        //else if (dif.y > 180)
        //{
        //    dif = new Vector3(dif.x, dif.y - 360, dif.z);
        //}
        //if (dif.y > _maxDeltaRotation.x)
        //{
        //    _sight.Rotate(0, _maxDeltaRotation.x - dif.y, 0);
        //}
        //else if(dif.y < -_maxDeltaRotation.x)
        //{
        //    _sight.Rotate(0, -_maxDeltaRotation.x - dif.y, 0);
        //}

        //  ピッチ制御
        //if (dif.x < -180)
        //{
        //    dif = new Vector3(dif.x + 360, dif.y, dif.z);
        //    Debug.Log($"#1");
        //}
        //else if (dif.x > 180)
        //{
        //    dif = new Vector3(dif.x - 360, dif.y, dif.z);
        //    Debug.Log($"#2");
        ////}
        //if (dif.x > _maxDeltaRotation.y)
        //{
        //    _sight.Rotate(_maxDeltaRotation.y - dif.x, 0, 0);
        //    //Debug.Log($"+");
        //}
        //else if (dif.x < -_maxDeltaRotation.y)
        //{
        //    _sight.Rotate(-_maxDeltaRotation.y - dif.x, 0, 0);
        //    //Debug.Log($"-");
        //}
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
