using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Entities.Steering
{
    public class MoveTo : BaseSteering
    {
        private float _initialSpeed = 0f;
        private float _currentSpeed = 0f;        

        private float _slowDownRadius = 10.0f;

        private float _timeTraveled = 0f;
        private float _timeToMaxSpeed = 3f;

        private float _timeToTarget = 0.1f;

        private Vector3 _destination;

        public Vector3 destination => _destination;

        public bool hasReachedDestination => Vector3.Distance(this.transform.position, _destination) < .5f;

        public void SetDestination(Vector3 destination)
        {            
            _destination = destination;
        }

        protected override void Update()
        {
            base.Update();

            Vector3 direction = (_destination - this.transform.position).normalized;
            float distance = Vector3.Distance(this.transform.position, _destination);
            Debug.DrawRay(this.transform.position, direction * distance, Color.green);
        }

        protected override SteeringVelocity GetSteering()
        {
            SteeringVelocity steering = new SteeringVelocity();
            if (this.hasReachedDestination)
                return steering;

            Vector3 direction = (_destination - this.transform.position).normalized;
            float distance = Vector3.Distance(this.transform.position, _destination);

            float targetAngle  = Vector3.Angle(direction, this.transform.forward);
            if (targetAngle > 1f)
            {
                float targetOrientation = Mathf.Atan2(direction.x, direction.z);
                targetOrientation *= Mathf.Rad2Deg;
                float rotation = targetOrientation - this.Entity.orientation;
                rotation = MapToRange(rotation);
                float rotationSize = Mathf.Abs(rotation);
                if (rotationSize > 2f)
                {
                    float targetRotation = this.Entity.maxRotation * rotation / rotationSize;
                    steering.angular = targetRotation - this.Entity.rotation;
                    steering.angular /= _timeToTarget;                    
                    float angularAcceleration = Mathf.Abs(steering.angular);
                    if (angularAcceleration > this.Entity.maxAngularAcceleration)
                    {
                        steering.angular /= angularAcceleration;
                        steering.angular *= this.Entity.maxAngularAcceleration;
                    }
                }
            }
            else
            {
                steering.angular = -this.Entity.rotation * this.Entity.maxAngularAcceleration;
            }

            if (distance > _slowDownRadius)
            {                
                _currentSpeed = Mathf.Lerp(_initialSpeed, this.Entity.maxSpeed, _timeTraveled / _timeToMaxSpeed);
                _timeTraveled += Time.deltaTime;
            }
            else
            {                
                _currentSpeed = this.Entity.maxSpeed * distance / _slowDownRadius;
                _initialSpeed = _currentSpeed;
                _timeTraveled = _initialSpeed * _timeToMaxSpeed / _timeTraveled;
            }

            Vector3 desiredVelocity = this.transform.forward;            
            desiredVelocity *= _currentSpeed;
            steering.linear = desiredVelocity - this.Entity.velocity;
            steering.linear /= _timeToTarget;

            if (steering.linear.magnitude > this.Entity.maxAcceleration)
            {
                steering.linear.Normalize();
                steering.linear *= this.Entity.maxAcceleration;
            }

            return steering;
        }

        private float MapToRange(float rotation) 
        {
            rotation %= 360.0f;
            if (Mathf.Abs(rotation) > 180.0f)
            {
                if (rotation < 0.0f)
                    rotation += 360.0f;
                else
                    rotation -= 360.0f;
            }
            return rotation;
        }
    }
}
