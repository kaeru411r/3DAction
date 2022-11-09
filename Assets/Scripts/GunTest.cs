using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTest : MonoBehaviour
{

    [SerializeField] Gun _gun;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Fire());
    }


    IEnumerator Fire()
    {
        while (true)
        {
            _gun.Fire();
            yield return new WaitForSeconds(/*_gun.Bullet.ReloadTime*/0);
        }
    }
}
