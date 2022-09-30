using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GunSystem : MonoBehaviour
{
    [Tooltip("�e��z��")]
    [SerializeField] List<Bullet> _ammos;
    [Tooltip("�C��")]
    [SerializeField] Transform _turret;
    [Tooltip("�C�g")]
    [SerializeField] Transform _barrel;
    [Tooltip("�C��")]
    [SerializeField] Transform _muzzle;
    [Tooltip("���ڏƏ������킹��")]
    [SerializeField] Transform _sight;
    [Tooltip("�C�̓���X�s�[�h[rad/s]")]
    [SerializeField] Vector2 _gunMoveSpeed;
    [Tooltip("�p")]
    [SerializeField, Range(0, 90)] float _elevationAngle = 0;
    [Tooltip("��p")]
    [SerializeField, Range(-90, 0)] float _depressionAngle = 0;
    /// <summary>�g���e��̎��</summary>
    int _ammoNunber;
    /// <summary>���ݑ��U����Ă邩</summary>
    bool _isLoad = true;
    /// <summary>���U�����܂ł̎���</summary>
    float _time;
    /// <summary>360�x</summary>
    const int AllAround = 360;
    /// <summary>�K�v��Transform���A�T�C������ĂȂ������Ƃ��̃_�~�[</summary>
    Transform _dummy;
    /// <summary>��Ԃ̃��W�b�h�{�f�B</summary>
    Rigidbody _rb;




    public Transform Sight { get { return _sight; } }

    public Transform Turret { get { return _turret; } }

    public Transform Barrel { get { return _barrel; } }

    public Transform Muzzle { get { return _muzzle; } }

    public Vector2 GunMoveSpeed { get { return _gunMoveSpeed; } }

    public Bullet Bullet
    {
        get
        {
            if (_ammos[_ammoNunber] != null)
            {
                return _ammos[_ammoNunber];
            }
            else
            {
                Debug.LogError($"{name}��{nameof(_ammos)}�ɂ�{_ammoNunber}�͂���܂���");
                return null;
            }
        }
    }

    private void Start()
    {
        if (_ammos.Count == 0)
        {
            Debug.LogError($"{name}��Ammos���I������Ă��܂���");
            _isLoad = false;
        }
        else if (_ammos.Contains(null))
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0, n = 0; i < _ammos.Count; i++, n++)
            {
                if (!_ammos[i])
                {
                    sb.Append($"\nElement{n}");
                    _ammos.RemoveAt(i);
                    i--;
                }
            }
            if (_ammos.Count == 0)
            {
                Debug.LogError($"{name}��Ammos���I������Ă��܂���");
                _isLoad = false;
            }
            else
            {
                sb.Insert(0, $"{name}��{this}��{nameof(_ammos)}�̈ȉ��̍��ڂɃA�T�C��������Ă��܂���");
                Debug.LogWarning(sb);
            }
        }
        if (!_turret)
        {
            Debug.LogError($"{name}��{nameof(_turret)}���A�T�C������Ă��܂���");
            _turret = PreDummy();
        }
        if (!_barrel)
        {
            Debug.LogError($"{name}��{nameof(_barrel)}���A�T�C������Ă��܂���");
            _barrel = PreDummy();
        }
        if (!_muzzle)
        {
            Debug.LogError($"{name}��{nameof(_muzzle)}���A�T�C������Ă��܂���");
            _muzzle = PreDummy();
        }
        if (!_sight)
        {
            Debug.LogError($"{name}��{nameof(_sight)}���A�T�C������Ă��܂���");
            _sight = PreDummy();
        }
        _rb = GetComponent<Rigidbody>();
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

    /// <summary>�C�e�̎��̉����甭�ˊ֐��̌Ăяo���܂ł��s��</summary>
    /// <param name="root"></param>
    /// <returns>���C�������ۂ�</returns>
    public bool Fire()
    {
        if (_isLoad)
        {
            Vector3 dir = new Vector3(_muzzle.eulerAngles.x + 90, _muzzle.eulerAngles.y, _muzzle.eulerAngles.z);
            var go = Instantiate(_ammos[_ammoNunber], _muzzle.position, _muzzle.rotation);
            go.GetComponent<Bullet>()?.Fire(transform);

            if (_rb)
            {
                float mass = go.Mass;
                _rb.AddForceAtPosition(-_muzzle.forward * mass * _ammos[_ammoNunber].Speed, _muzzle.position, ForceMode.Impulse);
            }

            StartCoroutine(Reload(_ammos[_ammoNunber].ReloadTime));
            return true;
        }
        return false;
    }

    /// <summary>�C�g�̏㉺�̓���</summary>
    /// <param name="x"></param>
    void Pitch(float x, float deltaTime)
    {
        var dif = x - _barrel.localEulerAngles.x;
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
            _barrel.localEulerAngles = new Vector3(x, 0, 0);
        }
        else if (dif > _gunMoveSpeed.y)
        {
            _barrel.Rotate(_gunMoveSpeed.y * deltaTime / Mathf.PI * 180, 0, 0);
        }
        else
        {
            _barrel.Rotate(-_gunMoveSpeed.y * deltaTime / Mathf.PI * 180, 0, 0);
        }
    }

    /// <summary>�C���̐���</summary>
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

    /// <summary>�C�e�̐؂�ւ����s��</summary>
    /// <param name="f"></param>
    public void Change(float f)
    {
        if (_ammos.Count != 0)
        {
            if (f < 0)
            {
                _ammoNunber = _ammoNunber - 1;
                if (_ammoNunber < 0)
                    _ammoNunber = _ammos.Count - 1;
            }
            else
            {
                _ammoNunber = (_ammoNunber + 1) % _ammos.Count;
            }
            StartCoroutine(Reload(_ammos[_ammoNunber].ReloadTime));
        }
    }

    public void Choice(int n)
    {
        if (_ammos.Count != 0)
        {
            if (n <= _ammos.Count && n - 1 != _ammoNunber)
            {
                _ammoNunber = n - 1;
                StartCoroutine(Reload(_ammos[_ammoNunber].ReloadTime));
            }
        }
    }


    /// <summary>�C�e�̑��U���s��
    /// �Ō�ɌĂяo����Ă���time�b��ɑ��U��������</summary>
    IEnumerator Reload(float time)
    {
        if (_isLoad)
        {
            Debug.Log($"{transform.root.name}��{_ammos[_ammoNunber].name}���U�����܂�{time}");
            _isLoad = false;
            for (_time = time; _time >= 0; _time -= Time.deltaTime)
            {
                yield return null;
            }
            Debug.Log($"{transform.root.name}�������[�h����");
            _isLoad = true;
        }
        else
        {
            _time = time;
            Debug.Log($"{transform.root.name}��{_ammos[_ammoNunber].name}���U�����܂�{_time}");
        }
    }

    Transform PreDummy()
    {
        if (_dummy)
        {
            return _dummy;
        }
        else
        {
            _dummy = new GameObject().transform;
            return _dummy;
        }
    }
}
