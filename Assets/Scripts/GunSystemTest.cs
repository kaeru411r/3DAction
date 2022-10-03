using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSystemTest : MonoBehaviour
{
    [SerializeField] GunSystem _gunSystem;

    // Start is called before the first frame update
    void Start()
    {
        _gunSystem.FireTimingMode = FireTimingMode.Coinstantaneous;
    }

    private void Update()
    {
        _gunSystem.Fire();
    }
}
