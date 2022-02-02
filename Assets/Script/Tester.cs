using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tester : MonoBehaviour
{
    [SerializeField] Transform _tr;
    private void Update()
    {
        Vector3 vec1 = _tr.position;
        Vector3 vec2 = transform.position;
        Vector2 xz = new Vector2(vec1.x, vec1.z) - new Vector2(vec2.x, vec2.z);

    }
}

[System.Serializable]
public class AxleInfo
{
}