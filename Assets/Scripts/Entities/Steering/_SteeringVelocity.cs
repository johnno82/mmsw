using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SteeringVelocity
{
    public Vector3 linear
    {
        get;
        set;
    }

    public float angular
    {
        get;
        set;
    }

    public SteeringVelocity()
    {
        this.angular = 0f;
        this.linear = Vector3.zero;
    }
}
