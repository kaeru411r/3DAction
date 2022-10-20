using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerCaterpillar : MonoBehaviour
{
    [SerializeField] CaterpillarController _caterpillarController;
    /// <summary>WASD及び左スティック</summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (_caterpillarController)
        {
            Vector2 move = context.ReadValue<Vector2>();
            //Call(move, _caterpillarController.Move);
            _caterpillarController.Move(move);
        }
        //Debug.LogError(_move);
    }
}
