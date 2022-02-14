using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : SingletonMonoBehaviour<ExplosionManager>
{
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
            foreach (var r in _simulationRbs)
            {
                r.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
            }
        }

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
}
