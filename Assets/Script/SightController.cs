using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightController : MonoBehaviour
{
    private void LateUpdate()
    {
        Vector3 v = Camera.main.transform.eulerAngles;
        v = new Vector3(v.x, v.y, 0);
        Camera.main.transform.eulerAngles = v;
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        //Debug.Log(transform.eulerAngles);
    }
}
