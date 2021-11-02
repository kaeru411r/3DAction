using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tester : CharacterBase
{
    [SerializeField] UnityEvent unityEvent;
    private void Start()
    {
        unityEvent.Invoke();
    }
}
