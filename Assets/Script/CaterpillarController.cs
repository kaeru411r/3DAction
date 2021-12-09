using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaterpillarController : MonoBehaviour
{
    [SerializeField] Wheels[] _wheels;
    [SerializeField] float _forwardTorque;
    [SerializeField] float _backTorque;
    [SerializeField] float _brakeTorque;
    [SerializeField] float _wheelDanpingRate;
    [SerializeField] ForwardFriction _forwardWheelFriction;
    [SerializeField] SideFriction _sideWheelFriction;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Reset()
    {

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
public class Wheels
{
    public float Mass;
    public float Radius;
    public float SuspensionDistance;
    public WheelCollider RightWheel;
    public WheelCollider LeftWheel;
    public float Spring;
    public float Damper;
    public float TargetPosition;
    public Vector3 Center;
}

[System.Serializable]
public class ForwardFriction
{
    public float ExtremumSlip;
    public float ExtremumValue;
    public float AsymptoteSlip;
    public float AsymptoteValue;
    public float Stiffness;
}

[System.Serializable]
public class SideFriction
{
    public float ExtremumSlip;
    public float ExtremumValue;
    public float AsymptoteSlip;
    public float AsymptoteValue;
    public float Stiffness;
}
