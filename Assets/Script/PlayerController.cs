using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
    [Tooltip("レティクル")]
    [SerializeField] Image _sight;
    [Tooltip("rayを飛ばす距離")]
    [SerializeField] float _distance;
    [Tooltip("照準するレイヤー")]
    [SerializeField] LayerMask _layerMask;

    private void Start()
    {
        _target = new GameObject().transform;
        _gunController.Target = _target;
    }
    // Update is called once per frame
    void Update()
    {
        float y = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        _characterBase.Move(z, y);
        if (Input.GetButtonDown("Fire1"))
        {
            _gunController.Fire(transform.root);
        }
        float f = Input.GetAxisRaw("Mouse ScrollWheel");
        if (f != 0)
        {
            _gunController.Choice(f);
        }

        Aim();
    }

    void Aim()
    {
        float centerX = Screen.width / 2;
        float centerY = Screen.height / 2;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(_sight.rectTransform.position);
        if (Physics.Raycast(ray, out hit, _distance, _layerMask)){
            _target.position = hit.point;
        }
        else
        {
            Vector3 direction = ray.direction;
            Vector3 position = ray.origin;
            _target.position = direction * _distance + position;
        }
    }

}
