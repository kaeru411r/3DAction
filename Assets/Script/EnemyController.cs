using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 敵の動作を管理する
/// </summary>
[RequireComponent(typeof(GunController), typeof(CaterpillarController))]
public class EnemyGunner : MonoBehaviour
{
    GunController _gunController;
    CaterpillarController _caterpillarController;
    // Start is called before the first frame update
    void Start()
    {
        _gunController = GetComponent<GunController>();
        _caterpillarController = GetComponent<CaterpillarController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
