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
    [Range(0.0f, 1.0f)] public float actionRangeRatio = 0.25f;
    [Range(0.0f, 5.0f)] public float attackDelay = 1;


    private Vector2 _center;
    private GameObject _joyStick = null;
    private bool _joyStickFlag = false;
    private int _move = -1;
    private int _attack = -1;

    private Vector2 _acenter;
    private bool _normalAttack = true;
    private float _attackRem = 0;

    void HandleMovement(Touch touch)
    {
        try
        {
            Vector2 pos = touch.position;
            if (touch.phase == TouchPhase.Began && !_joyStickFlag)
            {
                _move = touch.fingerId;
                _joyStickFlag = true;
                _center = Camera.main.ScreenToWorldPoint(pos);
                if (_joyStick != null && _joyStick.activeSelf)
                    Destroy(_joyStick);
                _joyStick = Instantiate(joyStickPrefab, _center, Quaternion.identity);
                _joyStick.transform.parent = gameObject.transform;
                _joyStick.name = "Joystick";
                MatchMessageHandler.i.SendLocation();
            }
            else if (touch.phase == TouchPhase.Moved && _joyStickFlag)
            {
                GameObject button = _joyStick.transform.GetChild(0).gameObject;
                Vector2 buttonPos = Camera.main.ScreenToWorldPoint(pos);
                _center = (Vector2) _joyStick.transform.position;
                Vector2 direction = buttonPos - _center;
                if (direction.magnitude > joyStickRange)
                    direction = direction.normalized * joyStickRange;

                button.transform.position = direction + _center;
                movementData.direction = direction.normalized;
                movementData.speed = direction.magnitude / joyStickRange;
                MatchMessageHandler.i.SendLocation();
            }
            else if (touch.phase == TouchPhase.Ended && _joyStickFlag)
            {
                movementData.speed = 0;
                _joyStickFlag = false;
                _move = -1;
                MatchMessageHandler.i.SendLocation();
                Destroy(_joyStick, 0.3f);
            }
        }
        catch (NullReferenceException)
        {
            Debug.Log("Camera Not Found");
        }
    }

    void HandleAction(Touch touch)
    {
        try
        {
            Vector2 pos = touch.position;
            if (touch.phase == TouchPhase.Began && !_joyStickFlag)
            {
                Debug.Log("First");
                _attack = touch.fingerId;
                _acenter = pos;
                _normalAttack = true;
                _attackRem = attackDelay;

            }
            else if (touch.phase == TouchPhase.Moved && _joyStickFlag)
            {
                Debug.Log("Second");
                if (Mathf.Abs(pos.y - _acenter.y) >= actionRangeRatio * Screen.height)
                {
                    _normalAttack = false;
                    if (pos.y - _acenter.y > 0)
                    {
                        //Use Item 1
                    }
                    else
                    {
                        //Use Item 2
                    }
                }
                else
                {
                    if (!_normalAttack)
                        _attackRem = attackDelay;
                    _normalAttack = true;
                }
            }
            else if (touch.phase == TouchPhase.Ended && _joyStickFlag)
            {
                _attack = -1;
                _normalAttack = true;
            }
        }
        catch (NullReferenceException)
        {
            Debug.Log("Camera Not Found");
        }
    }

    void CheckInput()
    {
        #if UNITY_EDITOR
        Touch temp = new Touch();
        temp.position = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            temp.phase = TouchPhase.Began;
            HandleMovement(temp);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            temp.phase = TouchPhase.Ended;
            HandleMovement(temp);
        }
        else if (Input.GetMouseButton(0))
        {
            temp.phase = TouchPhase.Moved;
            HandleMovement(temp);
        }
        #endif

        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).fingerId == _move)
            {
                HandleMovement(Input.GetTouch(i));
            }
            else if (Input.GetTouch(i).position.x < (Screen.width / 2) && _move == -1)
            {
                HandleMovement(Input.GetTouch(i));
            }
            
            if (Input.GetTouch(i).position.x >= (Screen.width / 2) && _attack == -1)
            {
                HandleAction(Input.GetTouch(i));
            }
        }
    }

    void Update()
    {
        CheckInput();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Shoot
            MatchMessageHandler.i.Shoot();
        }
        
    }

    IEnumerator OnAttackCheck()
    {
        WaitForSeconds delay = new WaitForSeconds(0.05f);
        while (true)
        {
            if (_normalAttack && _attackRem < 0)
            {
                //Normal Attack

            }
            
            _attackRem -= 0.05f;
            yield return delay;
        }
    }

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 50;
    }
}