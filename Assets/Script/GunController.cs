using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 砲動作コンポーネント
/// 砲塔以下の操作を行う
/// </summary>
public class GunController : MonoBehaviour
{

    [Tooltip("弾薬配列")]
    [SerializeField] BulletController[] _ammos;
    /// <summary>使う弾薬の種類</summary>
    int _ammoNunber;


    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
        float f = Input.GetAxisRaw("Mouse ScrollWheel");
        if (f != 0)
        {
            Choice(f);
        }
    }

    void Fire()
    {
        var go = Instantiate(_ammos[_ammoNunber], transform.position, transform.rotation);
        go.GetComponent<BulletController>()?.Fire(gameObject);
    }

    void Choice(float f)
    {
        if (f < 0)
        {

        }
    }
}
