using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
/// <summary>パンジャンの制御スクリプト</summary>
public class PanjandrumController : MonoBehaviour
{
    [Tooltip("ロケットの推進力")]
    [SerializeField] float _rocketPower;
    [Tooltip("爆発の基本ダメージ")]
    [SerializeField] float _damage;
    [Tooltip("爆風の吹き飛ばす力")]
    [SerializeField] float _blastPower;
    [Tooltip("爆風の到達半径")]
    [SerializeField] float _blastRadius;
    [Tooltip("ロケットのTransform")]
    [SerializeField] Transform[] _rockets;
    [Tooltip("爆発までの時間")]
    [SerializeField] float _time;
     
    Rigidbody _rb;
    bool _isFired;



    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        StartCoroutine(Timer(_time));
    }

    private void OnEnable()
    {
        if (!_rb)
        {
            _rb = GetComponent<Rigidbody>();
        }
        ExplosionManager.Instance.Add(_rb);
    }

    private void OnDisable()
    {
        ExplosionManager.Instance.Remove(_rb);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_isFired)
        {
            for (int i = 0; i < _rockets.Length; i++)
            {
                _rb.AddForceAtPosition(_rockets[i].forward * -1 * _rocketPower, _rockets[i].position);
            }
        }
    }

    public void Fire()
    {
        _isFired = true;
    }

    public void Blast()
    {
        ExplosionManager.Instance.Explosion(_blastPower, transform.position, _blastRadius, _damage);
        Destroy(gameObject);
    }

    IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
        Blast();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Fire();
    }
}
