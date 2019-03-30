using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkDriven : MonoBehaviour
{

    [Range(0.1f, 2)] public float timeError = 0.3f;

    private Vector3 _speed;
    private float _angleSpeed;
    private float _lastDataTime = 0;

    void Update()
    {
    }

    public void SetTarget(float x, float y, float angle)
    {
//        if (gameObject.name == "Crate 1")
//            Debug.Log(gameObject.name + ": " + (Time.time - _lastDataTime));
        Vector3 target = new Vector3(x, y, 0);
        transform.position = target;
        transform.eulerAngles = Vector3.forward * angle;
//        var trans = transform;
//        _speed = (target - trans.position) * 60;
//        _angleSpeed = (angle - trans.eulerAngles.z) * 60;
        _lastDataTime = Time.time;
    }
}