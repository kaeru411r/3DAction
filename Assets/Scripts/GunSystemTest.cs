using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSystemTest : MonoBehaviour
{
    [SerializeField] GunSystem _gunSystem;

    GameObjectCollector _collector;

    // Start is called before the first frame update
    void Start()
    {
        _collector = new GameObjectCollector(name);
        _gunSystem.FireTimingMode = FireTimingMode.Coinstantaneous;
    }

    private void Update()
    {
        List<Bullet> bullets = _gunSystem.Fire();
        if (bullets != null)
        {
            foreach (Bullet b in bullets)
            {
                _collector.Collection(b);
            }
        }
    }
}
