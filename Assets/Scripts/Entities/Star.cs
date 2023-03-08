using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : CelestialBody
{
    [field: SerializeField]
    private float _power = 1000;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        SetMainColor(Color.magenta);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}