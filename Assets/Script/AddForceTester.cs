using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AddForceTester : MonoBehaviour
{
    [SerializeField] bool _a;

    // Start is called before the first frame update
    void Start()
    {
        var rb = GetComponent<Rigidbody>();
        if (_a)
        {
            rb.AddForce(1, 0, 0, ForceMode.Impulse);
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                rb.AddForce(0.1f, 0, 0, ForceMode.Impulse);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
