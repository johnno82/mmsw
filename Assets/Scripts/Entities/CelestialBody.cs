using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody : BaseEntity
{
    [field: SerializeField]
    private float _size = 1;

    [field: SerializeField]
    private float _rotationSpeed = 0f;

    public void SetSize(float size)
    {
        _size = size;
        this.transform.localScale = new Vector3(size, size, size);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        this.SetMainColor(Color.blue);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
