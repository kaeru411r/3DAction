using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tester : MonoBehaviour
{
    public List<AxleInfo> axleInfos; // 個々の車軸の情報
    public float maxMotorTorque; //ホイールに適用可能な最大トルク
    public float maxSteeringAngle; // 適用可能な最大ハンドル角度
    [SerializeField] float MotorTorque;
    [SerializeField] float steeringAngle;

    public void FixedUpdate()
    {
        float motor = maxMotorTorque * MotorTorque;
        float steering = maxSteeringAngle * steeringAngle;

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
        }
    }
}

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; //このホイールがエンジンにアタッチされているかどうか
    public bool steering; // このホイールがハンドルの角度を反映しているかどうか
}