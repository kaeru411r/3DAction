using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tester : MonoBehaviour
{
    [SerializeField] int fps;
    private void Update()
    {
        Application.targetFrameRate = fps;
    }

}
