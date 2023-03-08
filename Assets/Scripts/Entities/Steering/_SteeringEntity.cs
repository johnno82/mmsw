using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringEntity : MonoBehaviour
{
    private SteeringVelocity _steeringVelocity = new SteeringVelocity();

    private float _trueMaxSpeed;

    private Vector3 _velocity;
    private float _orientation;
    private float _rotation;

    [field: SerializeField]
    private float _maxSpeed = 0f;

    [field: SerializeField]
    private float _maxAcceleration = 0f;

    [field: SerializeField]
    private float _maxRotation = 0f;

    [field: SerializeField]
    private float _maxAngularAcceleration = 0f;

    public Vector3 velocity => _velocity;

    public float orientation => _orientation;

    public float rotation => _rotation;

    public float maxSpeed => _maxSpeed;

    public float maxRotation => _maxRotation;

    public float maxAcceleration => _maxAcceleration;

    public float maxAngularAcceleration => _maxAngularAcceleration;

    public void SetMaxSpeed(float maxSpeed)
    {
        _maxSpeed = maxSpeed;
    }

    public void SetMaxAcceleration(float maxAcceleration)
    {
        _maxAcceleration = maxAcceleration;
    }
    public void SetMaxRotation(float maxRotation)
    {
        _maxRotation = maxRotation;
    }

    public void SetMaxAngularAcceleration(float maxAcceleration)
    {
        _maxAngularAcceleration = maxAcceleration;
    }

    public void SetSteering(SteeringVelocity steering, float weight)
    {
        _steeringVelocity.linear += (weight * steering.linear);
        _steeringVelocity.angular += (weight * steering.angular);
    }

    public void ResetMaxSpeed()
    {
        _maxSpeed = _trueMaxSpeed;
    }


    private void Start()
    {
        _steeringVelocity = new SteeringVelocity();

        _velocity = Vector3.zero;
        _trueMaxSpeed = _maxSpeed;
    }

    private void Update()
    {        
        if (_velocity != Vector3.zero)
        {
            Vector3 displacement = _velocity * Time.deltaTime;
            displacement.y = 0;
            this.transform.Translate(displacement, Space.World);
        }

        if (_rotation != 0)
        {
            // Limit orientation between 0 and 360
            _orientation += _rotation * Time.deltaTime;
            if (_orientation < 0.0f)
            {
                _orientation += 360.0f;
            }
            else if (_orientation > 360.0f)
            {
                _orientation -= 360.0f;
            }

            this.transform.rotation = new Quaternion();
            this.transform.Rotate(Vector3.up, _orientation);
        }

        _steeringVelocity.linear = Vector3.zero;
        _steeringVelocity.angular = 0;
    }

    private void LateUpdate()
    {
        if (_steeringVelocity != null)
        {
            _velocity += _steeringVelocity.linear * Time.deltaTime;
            _rotation += _steeringVelocity.angular * Time.deltaTime;

            if (_velocity.magnitude > _maxSpeed)
            {
                _velocity.Normalize();
                _velocity = _velocity * _maxSpeed;
            }

            if (_steeringVelocity.linear.magnitude == 0.0f)
                _velocity = Vector3.zero;

            if (_steeringVelocity.angular == 0f)
                _rotation = 0f;
        }
    }
}
