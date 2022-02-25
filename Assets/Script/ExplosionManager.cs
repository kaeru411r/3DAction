using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : SingletonMonoBehaviour<ExplosionManager>
{
    [Tooltip("爆発のシミュレーションの精度")]
    [SerializeField, Range(1, 31)] int _segment = 7;
    [SerializeField] LayerMask _layerMask;

    List<Rigidbody> _simulationRbs = new List<Rigidbody>();
    List<CharacterBase> _simulationCBs = new List<CharacterBase>();


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
        if (explosionRadius > 0)
        {
            RbNullCheck();
            CBNullCheck();
            float addDamage = 0;

            Dictionary<CharacterBase, float> damageCB = new Dictionary<CharacterBase, float>();

            explosionDamage /= _segment * _segment * _segment;
            explosionForce /= _segment * _segment * _segment;

            for (int i = 0; i <= _segment; i++)
            {
                for (int j = 0; j <= _segment; j++)
                {
                    for (int k = 0; k <= _segment; k++)
                    {
                        Vector3 dir = new Vector3(-1 + (i * (2f / _segment)), -1 + (j * (2f / _segment)), -1 + (k * (2f / _segment)));
                        Ray ray = new Ray(explosionPosition, dir.normalized);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit, explosionRadius))
                        {
                            var c = hit.collider.gameObject.GetComponent<CharacterBase>();
                            if (c)
                            {
                                float damage = (explosionRadius - Vector3.Distance(explosionPosition, hit.point)) / explosionRadius * explosionDamage;
                                damage = Mathf.Max(0, damage);
                                if (damageCB.ContainsKey(c))
                                {
                                    damageCB[c] += damage;
                                }
                                else
                                {
                                    damageCB.Add(c, damage);
                                }
                                addDamage += damage;
                            }

                            var r = hit.collider.gameObject.GetComponent<Rigidbody>();
                            if (r)
                            {
                                float force = (explosionRadius - Vector3.Distance(explosionPosition, hit.point)) / explosionRadius * explosionForce;
                                force = Mathf.Max(0, force);
                                if (_simulationRbs.Contains(r))
                                {
                                    r.AddForceAtPosition(ray.direction * force, hit.point, ForceMode.Impulse);
                                }
                            }
                        }
                    }
                }
            }
            foreach (var c in damageCB)
            {
                if (_simulationCBs.Contains(c.Key))
                {
                    c.Key.Shot(c.Value);
                }
            }
            return addDamage;
        }
        return 0;
    }

    private void AESimulate(float explosionForce, Vector3 explosionPosition, float explosionRadius, float explosionDamage, ref float addDamage, Ray ray, ref Dictionary<CharacterBase, float> damageCB)
    {
    }

    void CBNullCheck()
    {
        for (int i = 0; i < _simulationCBs.Count; i++)
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


