using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightController : MonoBehaviour
{
    private void Update()
    {
        Transform transform = GetComponent<Transform>();
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        Debug.Log(transform.eulerAngles);
    }
}
