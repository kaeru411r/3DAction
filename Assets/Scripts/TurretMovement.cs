using UnityEngine;
using System;
public class TurretMovement : MonoBehaviour
{
    /// <summary>�~�����p�x�ɕϊ�����</summary>
    public const float Mil2Deg = 0.05625f;
    public const float Rad2Mil = 0.00098174770424123456789123456789f;


    [Tooltip("����̃��[�h")]
    [SerializeField] Mode _turretMode = Mode.Unlimited;
    [Tooltip("��̃��[�h")]
    [SerializeField] Mode _pitchMode = Mode.Limited;
    [Tooltip("�T�C�g��Transform")]
    [SerializeField] Transform _sight;
    [Tooltip("��]���x")]
    [SerializeField] Vector2 _speed;
    [Tooltip("�C����Transform")]
    [SerializeField] Transform _turret;
    [Tooltip("�C�g��Transform")]
    [SerializeField] Transform _barrel;
    [Tooltip("�p")]
    [SerializeField] float _elevationAngle = 90f;
    [Tooltip("��p")]
    [SerializeField] float _depressionAngle = 90f;

    /// <summary>����̃��[�h</summary>
    public Mode TurretMode { get => _turretMode; set => _turretMode = value; }
    /// <summary>��̃��[�h</summary>
    public Mode PitchMode { get => _pitchMode; }
    /// <summary>�T�C�g��Transform</summary>
    public Transform Sight { get => _sight; set => _sight = value; }
    /// <summary>��]���x</summary>
    public Vector2 Speed { get => _speed; set => _speed = value; }
    /// <summary>�C����Transform</summary>
    public Transform TurretTransform { get => _turret; set => _turret = value; }
    /// <summary>�C�g��Transform</summary>
    public Transform BarrelTransform { get => _barrel; set => _barrel = value; }
    /// <summary>�p</summary>
    public float ElevationAngle { get => _elevationAngle; set => _elevationAngle = value; }
    /// <summary>��p</summary>
    public float DepressionAngle { get => _depressionAngle; set => _depressionAngle = value; }
    /// <summary>�Ə��ƖC�̊p�x�̌덷</summary>
    public float Misalignment { get => Vector3.Angle(_sight.forward, _barrel.forward) * Rad2Mil; }


    private void Update()
    {
        if (_sight && _turret)
        {
            Rotate(_turret, _sight.forward, _speed.x, Time.deltaTime);
            if (_barrel)
            {
                Pitch(_barrel, _turret.up, _sight.forward, _elevationAngle, _depressionAngle, _speed.y, Time.deltaTime);
            }
        }
    }

    /// <summary>
    /// ������v�Z����
    /// </summary>
    /// <param name="transform">�������I�u�W�F�N�g��Transform</param>
    /// <param name="forward">�T�C�g�̐��ʃx�N�g��</param>
    /// <param name="speed">���񑬓x</param>
    /// <param name="deltaTime">�o�ߎ���</param>
    /// <returns>��]���Quaternion</returns>
    void Rotate(Transform transform, in Vector3 forward, float speed, in float deltaTime)
    {
        //�C���ɑ΂���Ə���̐��ʂ̃x�N�g��
        Vector3 dir = transform.InverseTransformDirection(forward);
        //�C���ƏƏ����y���̊p�x�̍�
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        speed *= Mil2Deg;
        //����̏����œ������ׂ��p�x
        angle = (speed * deltaTime < Mathf.Abs(angle)) ? (speed * deltaTime * Mathf.Sign(angle)) : angle;

        //����rotation��K�v����]������Quaternion
        transform.Rotate(Vector3.up, angle);
    }

    /// <summary>
    /// �C�g�̓�����v�Z����
    /// </summary>
    /// <param name="selfTransform">�������I�u�W�F�N�g��Transform</param>
    /// <param name="baseAxis">��̉�]��(�����)</param>
    /// <param name="forward">�T�C�g�̐��ʃx�N�g��</param>
    /// <param name="elevation">�p�̐���</param>
    /// <param name="depression">��p�̐���</param>
    /// <param name="speed">���񑬓x</param>
    /// <param name="deltaTime">�o�ߎ���</param>
    void Pitch(Transform selfTransform, in Vector3 baseAxis, in Vector3 forward, float elevation, float depression, float speed, in float deltaTime)
    {
        elevation = 90f - elevation;
        depression += 90f;

        ////�����̊p�x[�x]
        float sightTheta = Mathf.Clamp(Vector3.Angle(baseAxis, forward), elevation, depression);
        float selfTheta = Vector3.Angle(baseAxis, selfTransform.forward);

        speed *= Mil2Deg;

        float y = (sightTheta - selfTheta);
        float angle = (speed * deltaTime < Mathf.Abs(y)) ? (speed * deltaTime * Mathf.Sign(y)) : y;

        selfTransform.Rotate(Vector3.right, angle);
    }


    public enum Mode
    {
        Static,
        Limited,
        Unlimited,
    }
}

