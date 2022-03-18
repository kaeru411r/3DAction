using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Tester : MonoBehaviour
{
    Vector2 _look = Vector2.zero;
    [SerializeField] TPSCamaraController _c;

    public void Aim(InputAction.CallbackContext context)
    {
        _look = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        _c.OnMouseLook(_look);
    }
}