using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GunSystem : MonoBehaviour
{
    /// <summary>360“x</summary>
    const int AllAround = 360;

    [Tooltip("–C”z—ñ")]
    [SerializeField] List<Gun> _guns;
    [Tooltip("–C“ƒ")]
    [SerializeField] Turret _turret;
    [Tooltip("’¼ÚÆ€‚ğ‡‚í‚¹‚éŠî€")]
    [SerializeField] Transform _sight;
    [Tooltip("–C‚Ì“®ìƒXƒs[ƒh[rad/s]")]
    [SerializeField] Vector2 _gunMoveSpeed;
    [Tooltip("‹ÂŠp")]
    [SerializeField, Range(0, 90)] float _elevationAngle = 0;
    [Tooltip("˜ëŠp")]
    [SerializeField, Range(-90, 0)] float _depressionAngle = 0;

    public Gun Gun { get => _guns[0]; }

    public Bullet Bullet { get => Gun.Bullet; }

    public Transform Sight { get { return _sight; } }

    public Transform Turret { get { return _turret.transform; } }

    public Transform Barrel { get { return Gun.Barrel; } }

    public Transform Muzzle { get { return Gun.Muzzle; } }

    public Vector2 GunMoveSpeed { get { return _gunMoveSpeed; } }

    private void Start()
    {
        if (!_sight)
        {
            Debug.LogError($"{name}‚Í{nameof(_sight)}‚ªƒAƒTƒCƒ“‚³‚ê‚Ä‚¢‚Ü‚¹‚ñ");
            _sight = Turret;
        }
    }

    private void Update()
    {
        float x = _sight.localEulerAngles.x;
        if (x > AllAround / 2)
        {
            x -= AllAround;
        }
        if (x < -_elevationAngle)
        {
            //Debug.Log($"#1 {_sight.eulerAngles.x} {-_elevationAngle} {_sight.eulerAngles.x < -_elevationAngle}");
            _sight.localEulerAngles = new Vector3(-_elevationAngle, _sight.localEulerAngles.y, _sight.localEulerAngles.z);
        }
        else if (x > -_depressionAngle)
        {
            //Debug.Log($"#2 {_sight.eulerAngles.x} {-_depressionAngle} {_sight.eulerAngles.x > _depressionAngle}");
            _sight.localEulerAngles = new Vector3(-_depressionAngle, _sight.localEulerAngles.y, _sight.localEulerAngles.z);
        }
    }


    private void FixedUpdate()
    {
        Yaw(_sight.localEulerAngles.y, Time.fixedDeltaTime);
        Pitch(_sight.localEulerAngles.x, Time.fixedDeltaTime);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_gunMoveSpeed.x < 0)
        {
            _gunMoveSpeed = new Vector2(0, _gunMoveSpeed.y);
        }
        if (_gunMoveSpeed.y < 0)
        {
            _gunMoveSpeed = new Vector2(_gunMoveSpeed.y, 0);
        }
    }

#endif

    /// <summary>–C’e‚ÌÀ‘Ì‰»‚©‚ç”­ËŠÖ”‚ÌŒÄ‚Ño‚µ‚Ü‚Å‚ğs‚¤</summary>
    /// <param name="root"></param>
    /// <returns>”­–C‚µ‚½‚©”Û‚©</returns>
    public bool Fire()
    {
        return Gun.Fire();
    }

    /// <summary>–C’e‚ÌØ‚è‘Ö‚¦‚ğs‚¤</summary>
    /// <param name="f"></param>
    public bool Change(float f)
    {
        return Gun.Change(f);
    }

    /// <summary>–C’e‚Ì‘I‘ğ‚ğs‚¤</summary>
    /// <param name="n"></param>
    public bool Choice(int n)
    {
        return Gun.Choice(n);
    }

    /// <summary>–Cg‚Ìã‰º‚Ì“®‚«</summary>
    /// <param name="x"></param>
    void Pitch(float x, float deltaTime)
    {
        var dif = x - Barrel.localEulerAngles.x;
        if (dif < -AllAround / 2)
        {
            dif = dif + AllAround;
        }
        else if (dif > AllAround / 2)
        {
            dif = dif - AllAround;
        }
        if (dif <= _gunMoveSpeed.y && dif >= -_gunMoveSpeed.y * deltaTime / Mathf.PI * 180)
        {
            Barrel.localEulerAngles = new Vector3(x, 0, 0);
        }
        else if (dif > _gunMoveSpeed.y)
        {
            Barrel.Rotate(_gunMoveSpeed.y * deltaTime / Mathf.PI * 180, 0, 0);
        }
        else
        {
            Barrel.Rotate(-_gunMoveSpeed.y * deltaTime / Mathf.PI * 180, 0, 0);
        }
    }

    /// <summary>–C“ƒ‚Ìù‰ñ</summary>
    /// <param name="y"></param>
    void Yaw(float y, float deltaTime)
    {
        var dif = y - _turret.transform.localEulerAngles.y;
        if (dif < -AllAround / 2)
        {
            dif = dif + AllAround;
        }
        else if (dif > AllAround / 2)
        {
            dif = dif - AllAround;
        }
        if (dif <= _gunMoveSpeed.x && dif >= -_gunMoveSpeed.x * deltaTime / Mathf.PI * 180)
        {
            _turret.transform.localEulerAngles = new Vector3(0, y, 0);
        }
        else if (dif > _gunMoveSpeed.x)
        {
            _turret.transform.Rotate(0, _gunMoveSpeed.x * deltaTime / Mathf.PI * 180, 0);
        }
        else
        {
            _turret.transform.Rotate(0, -_gunMoveSpeed.x * deltaTime / Mathf.PI * 180, 0);
        }
    }

}
