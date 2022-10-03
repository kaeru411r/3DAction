using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSystemTest : GameObjectCollector
{
    [SerializeField] GunSystem _gunSystem;

    // Start is called before the first frame update
    void Start()
    {
        _gunSystem.FireTimingMode = FireTimingMode.Coinstantaneous;
        base.Start();
    }

    private void Update()
    {
        List<Bullet> bullets = _gunSystem.Fire();
        if (bullets != null)
        {
            foreach (Bullet b in bullets)
            {
                Collection(b.transform);
            }
        }
    }
}
