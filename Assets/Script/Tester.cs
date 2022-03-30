using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Tester : MonoBehaviour
{
    [SerializeField] TPSCamaraController _tps;


    private void Update()
    {

    }

    public void Onlook(InputAction.CallbackContext context)
    {
        _tps.OnMouseLook(context.ReadValue<Vector2>());
    }
}