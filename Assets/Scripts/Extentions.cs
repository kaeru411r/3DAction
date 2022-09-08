using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Extentions
{
    public static bool GetComponentInParents<T>(this GameObject gameObject, T ob)
    {
        ob = gameObject.GetComponentInParent<T>();
        if (ob != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public static bool TryGetComponentInParent<T>(this Component gameObject, out T ob)
    {
        ob = gameObject.GetComponentInParent<T>();
        if(ob != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
