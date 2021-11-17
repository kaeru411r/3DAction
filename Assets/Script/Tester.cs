using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tester : MonoBehaviour
{
    [SerializeField] GunController GunController;
    private void Start()
    {
        GunController.Target = transform;
    }

}
