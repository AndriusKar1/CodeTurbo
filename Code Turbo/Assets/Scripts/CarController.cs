using NUnit.Framework;
using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;

public class CarController : MonoBehaviour
{
    // This is the car controller where it initialises all movement and wheels to make the car go vroom, the tutorial I used for this is https://www.youtube.com/watch?v=jr4eb4F9PSQ&list=PLyh3AdCGPTSLg0PZuD1ykJJDnC1mThI42 

    // 
    public enum Axel 
    {
        Front,
        Rear
    }
    //
    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public  Axel axel;
    }
    //
    public float maxAcceleration = 30.0f;
    public float breakAcceleration = 50.0f;

    public float turnSharpness = 1.0f;
    public float maxSteerAngle = 30.0f;

    public Vector3 _centerOfMass;


    public List<Wheel> wheels;

    float  moveInput;
    float  turnInput;

    private Rigidbody VehicleRB;
    //
    void Start()
    {
        VehicleRB = GetComponent<Rigidbody>();
        VehicleRB.centerOfMass = _centerOfMass;
    }
    //
    void FixedUpdate()
    {
        Move();
        Steer();
        Brake();
    }
    //
    void Update()
    {
        GetInputs();
        AnimatedWheels();
    }
    //
    void GetInputs() 
    {
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }
    //
    private void Move()
    {
        foreach (var wheel in wheels) 
        {
            wheel.wheelCollider.motorTorque = moveInput  * 50 *  maxAcceleration /** Time.deltaTime*/;
        }
    }
    //
    void Steer() 
    {
        foreach(var wheel in wheels) 
        {
            if (wheel.axel == Axel.Front) 
            {
                var _steerAngle = turnInput * turnSharpness * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 0.6f);
            }
        }
    }

    void Brake() 
    {
        if (Input.GetKey(KeyCode.Space)) { 
            foreach (var wheel in wheels) 
            {
                wheel.wheelCollider.brakeTorque = 300 * breakAcceleration;
            }
        }
        else 
        {
            foreach( var wheel in wheels) 
            {
                wheel.wheelCollider.brakeTorque = 0;
            }
        }
    }   

    void AnimatedWheels() 
    {
        foreach( var wheel in wheels) 
        {
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.wheelModel.transform.position = pos;
            wheel.wheelModel.transform.transform.rotation = rot;
        }
    }




}
