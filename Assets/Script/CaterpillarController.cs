using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaterpillarController : MonoBehaviour
{
    [SerializeField] Wheels[] _wheels;
    [SerializeField] float _forwardTorque;
    [SerializeField] float _backTorque;
    [SerializeField] float _brakeTorque;
    //[SerializeField] float _

    // Start is called before the first frame update
    void Start()
    {
        //_wheels[1].RightWheel.suspensionSpring.damper = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(Vector2 dir)
    {
        dir = dir.normalized;
        
    }
}

[System.Serializable]
public struct Wheels
{
    public WheelCollider RightWheel;
    public WheelCollider LeftWheel;
    public float Spring;
    public float Damper;
    public float TargetPosition;

}
