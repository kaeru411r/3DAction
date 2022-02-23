using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : SingletonMonoBehaviour<ExplosionManager>
{
    [Tooltip("爆発のシミュレーションの精度")]
    [SerializeField, Range(1, 16)] int _segment = 7;
    [SerializeField] LayerMask _layerMask;

    List<Rigidbody> _simulationRbs = new List<Rigidbody>();
    List<CharacterBase> _simulationCBs = new List<CharacterBase>();
    List<Z> z = new List<Z>();

    private void Update()
    {
        foreach(var s in z)
        {
            Debug.DrawRay(s.a, s.b);
        }
    }


    /// <summary>爆発の影響を受けるオブジェクトのリストに登録する</summary>
    /// <param name="rb"></param>
    public void Add(Rigidbody rb)
    {
        _simulationRbs.Add(rb);
    }

    /// <summary>爆発の影響を受けるオブジェクトのリストから除外する</summary>
    /// <param name="rb"></param>
    public void Remove(Rigidbody rb)
    {
        _simulationRbs.Remove(rb);
    }
    /// <summary>爆発のダメージを受けるオブジェクトのリストに登録する</summary>
    /// <param name="cB"></param>
    public void Add(CharacterBase cB)
    {
        _simulationCBs.Add(cB);
    }

    /// <summary>爆発のダメージを受けるオブジェクトのリストから除外する</summary>
    /// <param name="cB"></param>
    public void Remove(CharacterBase cB)
    {
        _simulationCBs.Remove(cB);
    }

    /// <summary>
    /// リストに登録されているRigidbody全てのAddExplosionForceを呼ぶ
    /// </summary>
    /// <param name="explosionForce"></param>
    /// <param name="explosionPosition"></param>
    /// <param name="explosionRadius"></param>
    public void Explosion(float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        RbNullCheck();
        foreach (var r in _simulationRbs)
        {
            r.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
        }
    }

    /// <summary>
    /// リストにある全ての対象に爆発の影響を与える
    /// </summary>
    /// <param name="explosionForce"></param>
    /// <param name="explosionPosition"></param>
    /// <param name="explosionRadius"></param>
    /// <param name="explosionDamage"></param>
    /// <returns></returns>
    public float Explosion(float explosionForce, Vector3 explosionPosition, float explosionRadius, float explosionDamage)
    {
        float damage = 0;
        if (explosionForce > 0)
        {
            RbNullCheck();
            foreach (var r in _simulationRbs)
            {
                r.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
            }
        }
        CBNullCheck();
        foreach (var c in _simulationCBs)
        {
            float d = (explosionRadius - Vector3.Distance(explosionPosition, c.transform.position)) / explosionRadius * explosionDamage;
            if (d > 0)
            {
                c.Shot(d);
                damage += d;
            }
        }
        return damage;
    }

    public float AdvancedExplosion(float explosionForce, Vector3 explosionPosition, float explosionRadius, float explosionDamage)
    {
        RbNullCheck();
        CBNullCheck();
        float addDamage = 0;

        for (int i = 0; i <= _segment; i++)
        {
            for(int j = 0; j <= _segment; j++)
            {
                for(int k = 0; k <= _segment; k++)
                {
                    RaycastHit hit;
                    Vector3 dir = new Vector3(-1 + (i * (2f / _segment)), -1 + (j * (2f / _segment)), -1 + (k * (2f / _segment)));
                    Ray ray = new Ray(explosionPosition, dir.normalized);
                    if(Physics.Raycast(ray, out hit, explosionRadius)){
                        var c = hit.collider.gameObject.GetComponent<CharacterBase>();
                        if (c)
                        {
                            float damage = explosionRadius - Vector3.Distance(explosionPosition, hit.point) / explosionRadius * explosionDamage;
                            damage = Mathf.Max(0, damage);
                            if (_simulationCBs.Contains(c))
                            {
                                c.Shot(damage);
                                addDamage += damage;
                            }
                        }

                        var r = hit.collider.gameObject.GetComponent<Rigidbody>();
                        if (r)
                        {
                            float force = (explosionRadius - Vector3.Distance(explosionPosition, hit.point) / explosionRadius) * explosionForce;
                            force = Mathf.Max(0, force);
                            if (_simulationRbs.Contains(r))
                            {
                                Debug.Log(force);
                                z.Add(new Z(hit.point, ray.direction * force));
                                r.AddForceAtPosition(hit.point, ray.direction * force);
                            }
                        }
                    }
                }
            }
        }

        return addDamage;
    }

    void CBNullCheck()
    {
        for(int i = 0; i < _simulationCBs.Count; i++)
        {
            if (!_simulationCBs[i])
            {
                _simulationCBs.RemoveAt(i);
                i--;
            }
        }
    }
    void RbNullCheck()
    {
        for (int i = 0; i < _simulationRbs.Count; i++)
        {
            if (!_simulationRbs[i])
            {
                _simulationRbs.RemoveAt(i);
                i--;
            }
        }
    }
}

public struct Z
{
    public Vector3 a;
    public Vector3 b;
    public Z(Vector3 a, Vector3 b)
    {
        this.a = a;
        this.b = b;
    }
}
