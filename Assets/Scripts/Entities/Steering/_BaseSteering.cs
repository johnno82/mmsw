using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSteering : MonoBehaviour
{
    protected SteeringEntity _entity;

    private float _weight = 1.0f;
    
    public SteeringEntity Entity => _entity;

    public void SetWeight(float weight)
    {
        _weight = weight;
    }

    protected virtual void Start()
    {
        _entity = gameObject.GetComponent<SteeringEntity>();

    }

    protected virtual void Update()
    {
        _entity.SetSteering(GetSteering(), _weight);
    }

    protected abstract SteeringVelocity GetSteering();

}