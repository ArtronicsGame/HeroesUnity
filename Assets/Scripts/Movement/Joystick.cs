using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Joystick : MonoBehaviour
{
    public MovementData movementData;
    public GameObject joyStickPrefab;
    [Range(1.0f, 10.0f)] public float joyStickRange = 3.0f;

    private Vector2 _center;
    private GameObject _joyStick = null;
    private bool _joyStickFlag = false;
    private bool _move;
    private bool _attack;

    void HandleMovement(Touch touch)
    {
        try
        {
            _move = true;
            Vector2 pos = touch.position;
            if (touch.phase == TouchPhase.Began && !_joyStickFlag)
            {
                _joyStickFlag = true;
                _center = Camera.main.ScreenToWorldPoint(pos);
                Debug.Log(_center);
                if (_joyStick != null && _joyStick.activeSelf)
                    Destroy(_joyStick);
                _joyStick = Instantiate(joyStickPrefab, _center, Quaternion.identity);
                _joyStick.transform.parent = gameObject.transform;
                _joyStick.name = "Joystick";
            }
            else if (touch.phase == TouchPhase.Moved && _joyStickFlag)
            {
                GameObject button = _joyStick.transform.GetChild(0).gameObject;
                Vector2 buttonPos = Camera.main.ScreenToWorldPoint(pos);
                _center = (Vector2) _joyStick.transform.position;
                Vector2 direction = buttonPos - _center;
                if (direction.magnitude > joyStickRange)
                {
                    direction = direction.normalized * joyStickRange;
                }

                button.transform.position = direction + _center;
                movementData.direction = direction.normalized;
                movementData.speed = direction.magnitude / joyStickRange;
            }
            else if (touch.phase == TouchPhase.Ended && _joyStickFlag)
            {
                movementData.speed = 0;
                _joyStickFlag = false;
                Destroy(_joyStick, 0.3f);
            }
        }
        catch (NullReferenceException)
        {
            Debug.Log("Camera Not Found");
        }
    }


    void Update()
    {
        _move = false;
        _attack = false;
        
        #if UNITY_EDITOR
        Touch temp = new Touch();
        temp.position = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            temp.phase = TouchPhase.Began;
            HandleMovement(temp);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            temp.phase = TouchPhase.Ended;
            HandleMovement(temp);
        }
        else if(Input.GetMouseButton(0))
        {
            temp.phase = TouchPhase.Moved;
            HandleMovement(temp);
        }
        #endif
        
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).position.x < (Screen.width / 2) && !_move)
            {
                HandleMovement(Input.GetTouch(i));
            }
            else if (Input.GetTouch(i).position.x > (Screen.width / 2) && !_attack)
            {
            }
        }
    }

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 50;
    }
}