﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using PoisonArch;
using PoisonArch.Mechanics;

using MoreMountains.NiceVibrations;
using Dreamteck.Splines;

public class PlayerController : MonoBehaviour, IEventScripts
{
    public static Player Instance => s_Instance;
    static Player s_Instance;

    [Header ("MOVEMENT")]
    [SerializeField] bool SwipeLeftRight;
    float movement;
    public float verticalSpeed = 12f;
    //public float horizontalSpeed = 1f;
    [SerializeField] float _turnSpeed = 450f;
    [SerializeField] float _speed = 5f;
    [SerializeField] float PlatformLeftBorder;
    [SerializeField] float PlatformRightBorder;
    [SerializeField] float rotateAngle;

    [Header("DREAMTECK SPLINE MOVEMENT")]
    [HideInInspector] public bool _isLaunched = true;
    SwipeMovement SM = new SwipeMovement();

    bool _walk = true;


    SplineFollower _splineFollower;
    public bool useSpline;
    public SplineFollower SplineFollower { get { return _splineFollower; } }

    private void Start()
    {
        GameManager.Instance.EventPlay += OnPlay;
        GameManager.Instance.EventMenu += OnMenu;
        GameManager.Instance.EventLose += OnLose;
        GameManager.Instance.EventFinish += OnFinish;

        if (useSpline)
        {
            _splineFollower = GetComponent<SplineFollower>();
        }
    }

    void Update()
    {
        SplineMovement();

        Movement();
    }
    void Movement()
    {
        if (GameManager.Instance.GameState == GameState.Play && _walk)
        {
            if (!useSpline)
            {
                transform.Translate(Vector3.forward * _speed * Time.deltaTime);
                if (_isLaunched)
                {
                    transform.position = new Vector3(Mathf.Clamp(transform.position.x, PlatformLeftBorder, PlatformRightBorder), transform.position.y, transform.position.z);

                    if (SwipeLeftRight)
                    {
                        transform.position += new Vector3(SM.GetRotationMagnitude() * _turnSpeed * Time.deltaTime, 0, 0);

                        movement = Mathf.Lerp(movement, SM.GetMovementMagnitude(), 5 * Time.fixedDeltaTime);
                        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(
                            new Vector3(Mathf.Clamp(movement, -1f, 1f), 0, 0.02f)), 10 * Time.fixedDeltaTime);
                    }
                    else
                    {
                        transform.position += new Vector3(SM.GetMovementMagnitude() * _turnSpeed * Time.deltaTime, 0, 0);
                    }
                }
            }
        }

        SetRotation();
    }
    private void SetRotation()
    {
        if (transform.eulerAngles.y >= rotateAngle && transform.eulerAngles.y < 180)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotateAngle, transform.eulerAngles.z);
        }
        else if (transform.eulerAngles.y <= 360 - rotateAngle && transform.eulerAngles.y > 180)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 360 - rotateAngle, transform.eulerAngles.z);
        }
    }
    void SplineMovement()
    {
        if (GameManager.Instance.GameState == GameState.Play && _walk)
        {
            if (useSpline)
            {
                _splineFollower.motion.offset =
                   new Vector2(Mathf.Clamp(_splineFollower.motion.offset.x + SM.GetMovementMagnitude() * _turnSpeed * Time.deltaTime, PlatformLeftBorder, PlatformRightBorder), _splineFollower.motion.offset.y);

            }
        }
    }
    public void OnMenu()
    {
        this.GetComponent<BoxCollider>().enabled = true;

        //splines
        if (useSpline)
        {
            SplineFollower.enabled = true;
            _isLaunched = false;
            _splineFollower.follow = false;
            _splineFollower.spline = FindObjectOfType<SplineComputer>();
            _splineFollower.Restart(0);
        }
    }
    public void OnPlay()
    {
        _walk = true;

        if (useSpline)
        {
            StartCoroutine(SplineInitPlayer());
        }
    }
    public void OnFinish()
    {
        MMVibrationManager.Haptic(HapticTypes.HeavyImpact);

        _walk = false;
    }
    public void OnLose()
    {
        MMVibrationManager.Haptic(HapticTypes.HeavyImpact);

        _walk = false;
    }
    IEnumerator SplineInitPlayer()
    {
        yield return new WaitForSeconds(0f);
        _isLaunched = true;
        _splineFollower.spline = LevelManager.Instance.GetCurrentLevel().transform.GetChild(0).GetComponent<SplineComputer>();
        _splineFollower.follow = true;

    }
}

/* SAMPLE
       private void OnTriggerEnter(Collider collision)
   {
       if (collision.gameObject.CompareTag("Final"))
       {
           Player.Instance.SetBoolAnimation(PlayerState.Run, false);
           Player.Instance.SetTriggerAnimation(PlayerState.Idle);

           _isLaunched = false;
           _walk = false;

           GameManager.Instance.SetFinish();

       }
       if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
       {
           if (!die)
           {
               Player.Instance.SetTriggerAnimation(PlayerState.Lose);
               //Player.Instance.SetBoolAnimation(PlayerState.AfterRun, true);
               this.GetComponent<BoxCollider>().enabled = false;
               die = true;
           }
           _isLaunched = false;
           GameManager.Instance.SetLose();
       }

   }
  */