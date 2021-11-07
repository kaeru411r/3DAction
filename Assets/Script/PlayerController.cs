using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// プレイヤー操作コンポーネント
/// 車両の操作を行う
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] GunController _gunController;
    [SerializeField] CharacterBase _characterBase;
    Transform _transform;


    // Update is called once per frame
    void Update()
    {
        float y = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        _characterBase.Move(z, y);
        if (Input.GetButtonDown("Fire1"))
        {
            _gunController.Fire(_transform);
        }
        float f = Input.GetAxisRaw("Mouse ScrollWheel");
        if (f != 0)
        {
            _gunController.Choice(f);
        }
    }

    private void Start()
    {
        _transform = transform.GetComponentInChildren<Transform>();
    }

}
