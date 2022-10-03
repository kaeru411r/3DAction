using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSystemTest : MonoBehaviour
{
    [SerializeField] GunSystem _gunSystem;

    Transform _stockTransform;

    // Start is called before the first frame update
    void Start()
    {
        _gunSystem.FireTimingMode = FireTimingMode.Coinstantaneous;
        _stockTransform = new GameObject().transform;
        _stockTransform.name = $"{name}Bullets";
    }

    private void Update()
    {
        List<Bullet> bullets = _gunSystem.Fire();
        if (bullets != null)
        {
            foreach (Bullet b in bullets)
            {
                b.transform.SetParent(_stockTransform);
            }
        }
    }
}
