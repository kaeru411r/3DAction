using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TestCameraChange : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCameraBase _intermediateVCam;
    [SerializeField] CinemachineVirtualCameraBase _freelookVCam;
    [SerializeField] Transform _muzzle;


    public void FpsToFreelook()
    {
        StartCoroutine(FpsToFreelookRoutine());
    }

    IEnumerator FpsToFreelookRoutine()
    {
        _intermediateVCam.MoveToTopOfPrioritySubqueue();
        yield return null;
        _freelookVCam.MoveToTopOfPrioritySubqueue();
    }
}
