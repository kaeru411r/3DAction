using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Tester : MonoBehaviour
{
    void Update()
    {
        // ゲームパッドが接続されていないとnullになる。
        if (Keyboard.current == null) return;

        //if (Keyboard.current.buttonNorth.wasPressedThisFrame)
        //{
        //    Debug.Log("Button Northが押された！");
        //}
        //if (Keyboard.current.buttonSouth.wasReleasedThisFrame)
        //{
        //    Debug.Log("Button Southが離された！");
        //}
    }

    void OnGUI()
    {
        if (Keyboard.current == null) return;

        GUILayout.Label($"leftStick: {Keyboard.current.aKey.isPressed}");
    }

}
