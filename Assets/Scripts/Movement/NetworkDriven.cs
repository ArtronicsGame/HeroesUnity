using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkDriven : MonoBehaviour
{

    void Update()
    {
    }

    public void SetTarget(float x, float y, float angle)
    {
        Vector3 target = new Vector3(x, y, 0);
        transform.position = target;
        transform.eulerAngles = Vector3.forward * angle;
    }
}
