using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
public class PlayerController : Singleton<PlayerController>
{
    Rigidbody _rigidBody;

    Vector2 _inputStart;
    Vector2 _inputEnd;

    Vector3 _direction;

    [SerializeField] float _turnSpeed = 5f;
    [SerializeField] float _speed = 5f;
    [SerializeField] float _forceMultiplier = 500f;
    [SerializeField] float PlatformLeftBorder;
    [SerializeField] float PlatformRightBorder;
    float angle;

    public bool RudderLock = true;

    void Start()
    {
        //_rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(RudderLock)
            PlayerInput();

    }

    void FixedUpdate()
    {
        Movement();
    }

    void Movement() 
    {
        /*if (Player.Instance._playerState != PlayerState.Run)
            return;*/

        transform.Translate(Vector3.forward * _speed * Time.fixedDeltaTime);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x + _direction.x * Time.fixedDeltaTime * _turnSpeed, PlatformLeftBorder, PlatformRightBorder), transform.position.y,transform.position.z);

        float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * 2f * _direction.magnitude);
        transform.eulerAngles = Vector3.up * angle;

        Vector3 charAngle = transform.eulerAngles;
        charAngle.y = (charAngle.y > 180) ? charAngle.y - 360 : charAngle.y;
        charAngle.y = Mathf.Clamp(charAngle.y, -15f, 15f);

        transform.rotation = Quaternion.Euler(charAngle);
    }

    void PlayerInput()
    {
        if (Input.GetMouseButton(0))
        {
            _inputStart = Input.mousePosition;
        }


        if (Input.GetMouseButtonDown(0))
        {
            _inputEnd = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _inputStart = Vector2.zero;
            _inputEnd = Vector2.zero;
        }

        if (_inputStart.x - _inputEnd.x < 0) //left
        {
            _direction = Vector3.left;
        }
        else if (_inputStart.x - _inputEnd.x > 0) //right
        {
            _direction = Vector3.right;
        }
        else
        {
            _direction = Vector3.zero;
        }
    }
}
