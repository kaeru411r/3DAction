using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSystemTest : MonoBehaviour
{
    [SerializeField] GunSystem _gunSystem;
    [SerializeField] FireTimingMode _fireTimingMode = FireTimingMode.Coinstantaneous;

    GameObjectCollector _collector;

    // Start is called before the first frame update
    void Start()
    {
        _collector = new GameObjectCollector($"{name}Collector");
        var b = _gunSystem.Barrel;
    }

    private void Update()
    {
        _gunSystem.FireTimingMode = _fireTimingMode;
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
