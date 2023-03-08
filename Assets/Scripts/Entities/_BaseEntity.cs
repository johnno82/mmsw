using Assets.Scripts.Entities.Steering;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public abstract class BaseEntity : MonoBehaviour
{
    private const float DefaultStopDistance = 5f;

    private Color _mainColor;

    [field: SerializeField]
    private int _lifePoints = 0;

    [field: SerializeField]
    private string _fullName = "Unknown";

    [field: SerializeField]
    private float _maxSpeed;

    [field: SerializeField]
    private float _maxAccelaration;

    [field: SerializeField]
    private float _maxRotation;

    [field: SerializeField]
    private float _maxAngularAccelaration;

    protected MeshRenderer meshRenderer { get; private set; }

    protected SteeringEntity steering { get; private set; }

    [field: SerializeField]
    public bool isSelectable { get; set; }

    public bool isSelected { get; set; }

    [field: SerializeField]
    public bool isKillable { get; set; }

    public bool isKilled => this.isKillable && _lifePoints < 0;

    public bool IsMovable => _maxSpeed > 0f && _maxAccelaration > 0f;

    public bool HasDestination => GetComponent<MoveTo>() != null;

    public void SetFullName(string name)
    {
        _fullName = name;
    }

    public void SetMainColor(Color color)
    {
        _mainColor = color;
        this.meshRenderer.material.color = color;
    }

    public void HitWithDamage(int damage)
    {
        if (!this.isKillable)
            return;

        if(_lifePoints > 0)
            _lifePoints -= damage;
    }

    public void SetDestination(Vector3 destination)
    {
        MoveTo moveTo = GetComponent<MoveTo>();
        if (moveTo == null)
            moveTo = this.gameObject.AddComponent<MoveTo>();

        moveTo.SetDestination(destination);
        moveTo.SetWeight(1f);        
    }


    protected virtual void Awake()
    {
        this.meshRenderer = GetComponent<MeshRenderer>();

        if (this.IsMovable)
            this.steering = this.gameObject.AddComponent<SteeringEntity>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _mainColor = Color.white;

        this.meshRenderer.material.color = _mainColor;

        if (this.IsMovable)
        {
            this.steering.SetMaxSpeed(_maxSpeed);
            this.steering.SetMaxAcceleration(_maxAccelaration);
            this.steering.SetMaxRotation(_maxRotation);
            this.steering.SetMaxAngularAcceleration(_maxAngularAccelaration);
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        this.meshRenderer.material.color = this.isSelected ? Color.yellow : _mainColor;

        if(this.IsMovable && this.HasDestination)
        {
            MoveTo moveTo = GetComponent<MoveTo>();
            if(moveTo.hasReachedDestination)
                DestroyImmediate(moveTo);            
        }

        if(this.IsMovable)
            Debug.DrawRay(this.transform.position, this.transform.forward * 2f, Color.yellow);
    }
}
