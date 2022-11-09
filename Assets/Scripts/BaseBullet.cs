using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBullet : MonoBehaviour
{
    public virtual float Speed { get { return float.PositiveInfinity; } }

    public virtual float Gravity { get { return 0; } }

    public virtual float Mass { get { return 0; } }

    public virtual float ReloadTime { get { return 0; } }

    public abstract GameObject Fire(Transform root);
}
