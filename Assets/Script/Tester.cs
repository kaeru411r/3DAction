using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tester : CharacterBase
{
    private void Update()
    {
        Debug.Log(transform.localEulerAngles);
    }

}
