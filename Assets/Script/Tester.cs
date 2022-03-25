using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Tester : MonoBehaviour
{
    [SerializeField] Transform _a = default;
    [SerializeField] Transform _b = default;


    private void Update()
    {
        //Followを中心とし、カメラ上を通る円の回転軸の方向ベクトル
        Vector3 v0 = _a.forward;
        Debug.DrawRay(_a.position, v0, Color.red);
        Debug.DrawLine(_a.position + _a.right * 100, _a.position + _a.right * -100, Color.blue);
        Debug.DrawLine(_a.position + _a.up * 100, _a.position + _a.up * -100, Color.blue);
        float d0 = -(-v0.x * _a.position.x - v0.y * _a.position.y - v0.z * _a.position.z);
        //制限の円の回転軸の方向ベクトル
        Vector3 v1 = _b.forward;
        Vector3 cPos = _b.position;
        Debug.DrawRay(cPos, v1, Color.red);
        Debug.DrawLine(cPos + _b.right * 100, cPos + _b.right * -100, Color.blue);
        Debug.DrawLine(cPos + _b.up * 100, cPos + _b.up * -100, Color.blue);
        float d1 = -(-v1.x * cPos.x - v1.y * cPos.y - v1.z * cPos.z);
        //各円を含む二つの面の接線の方向ベクトル
        Vector3 e = new Vector3(v0.y * v1.z - v0.z * v1.y, v0.z * v1.x - v0.x * v1.z, v0.x * v1.y - v0.y * v1.x);

        Vector3 a = Vector3.zero;

        if (e.z != 0)
        {
            a = new Vector3((d0 * v1.y - d1 * v0.y) / e.z, (d0 * v1.x - d1 * v0.x) / (-e.z), 0);
            Debug.Log($"1 {e}");
        }
        else if (e.y != 0)
        {
            a = new Vector3((d0 * v1.z - d1 * v0.z) / (-e.y), 0, (d0 * v1.x - d1 * v0.x) / e.y);
            Debug.Log($"2 {e}");
        }
        else if (e.x != 0)
        {
            a = new Vector3(0, (d0 * v1.z - d1 * v0.z) / e.x, (d0 * v1.y - d1 * v0.y) / (-e.x));
            Debug.Log($"3 {e}");
        }

        Debug.DrawLine(a + e * 100, a + e * -100);
    }
}