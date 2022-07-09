using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Tester : MonoBehaviour
{
    Vector2 _look;
    TPSCamaraController _TPSCamaraController;

    public void OnLook(InputAction.CallbackContext callbackContext)
    {
        _look = callbackContext.ReadValue<Vector2>();
    }

    private void Start()
    {
        _TPSCamaraController = GetComponent<TPSCamaraController>();
    }

    private void Update()
    {
        //if (_TPSCamaraController)
        //{
        //    _TPSCamaraController.OnMouseLook(_look);
        //}
        //else
        //{
        //    //Debug.LogError(1);
        //}
    }
}