using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckMovement : MonoBehaviour
{
    public float _MoveSpeed;
    public float _MaxSpeed;
    private float _deltaSpeed;
    public float _MinAcseleration;
    public float _PercentAcseleration;

    public float _VectorDirection;
    private float _CurrentDirection;
    public float _RotationSpeed;
    private float _DirectionDelta;
    public float _CollisionDelta;

    public GameObject canvaspad;
    private MenuUIController wincontroller;

    private Vector3 ConnectorPoint;
    private float PreviousRotation;
    private float DeltaRotation;
    private float ThisRotation;
    public List<Transform> Tails;
    public GameObject TailPrefab;

    private void Awake()
    {
        wincontroller = canvaspad.GetComponent<MenuUIController>();
    }
    void Update()
    {
        MoveTruck();
        if (Mathf.Abs(_VectorDirection - _CurrentDirection) > 1)
        {
            TruckRotation();
        }
        ConnectorPoint = transform.position - (transform.forward * 3f);
        PreviousRotation = transform.eulerAngles.y;
        MoveTails();
    }
    private void MoveTruck()
    {
        if (_MoveSpeed < _MaxSpeed)
        {
            _deltaSpeed = _MaxSpeed - _MoveSpeed;
            _MoveSpeed += (_deltaSpeed * _PercentAcseleration + _MinAcseleration) * Time.deltaTime;
        }
        else if (_MoveSpeed > _MaxSpeed * 1.05)
        {
            _MoveSpeed = _MaxSpeed;
        }
        transform.Translate(_MoveSpeed * Time.deltaTime * Vector3.forward);
    }
    private void TruckRotation()
    {
        if (_VectorDirection - 180 > _CurrentDirection)
        {
            _CurrentDirection += 360;
        }
        else if (_VectorDirection + 180 < _CurrentDirection)
        {
            _CurrentDirection -= 360;
        }
        if (_VectorDirection > _CurrentDirection)
        {
            _CurrentDirection += _RotationSpeed * Time.deltaTime;
        }
        else
        {
            _CurrentDirection -= _RotationSpeed * Time.deltaTime;
        }
        transform.rotation = Quaternion.Euler(0f, -_CurrentDirection, 0f);
    }
    private void MoveTails()
    {
        foreach (var Cargo in Tails)
        {
            Cargo.position = ConnectorPoint;
            ThisRotation = Cargo.eulerAngles.y;
            DeltaRotation = PreviousRotation - ThisRotation;
            if (Mathf.Abs(DeltaRotation) > 1)
            {
                if (DeltaRotation > 180)
                {
                    ThisRotation += 360;
                    DeltaRotation = PreviousRotation - ThisRotation;
                }
                else if (DeltaRotation < -180)
                {
                    ThisRotation -= 360;
                    DeltaRotation = PreviousRotation - ThisRotation;
                }
                if (PreviousRotation > ThisRotation)
                {
                    ThisRotation += _RotationSpeed * Time.deltaTime * (DeltaRotation * 0.04f + 0.2f);
                }
                else
                {
                    ThisRotation += _RotationSpeed * Time.deltaTime * (DeltaRotation * 0.04f - 0.2f);
                }
            }
            Cargo.transform.rotation = Quaternion.Euler(0f, ThisRotation, 0f);
            PreviousRotation = ThisRotation;
            ConnectorPoint = Cargo.position - (Cargo.forward * 5f);
        }
    }
    private void OnCollisionEnter(Collision collisionEnter)
    {
        if (collisionEnter.gameObject.name != "Earth")
        {
            _DirectionDelta = 0 - transform.eulerAngles.y;
            _MoveSpeed = 5f;
        }
    }
    private void OnCollisionStay(Collision collisionStay)
    {
        if (collisionStay.gameObject.name != "Earth")
        {
            _CurrentDirection = 0 - transform.eulerAngles.y;
            _MoveSpeed = 5f;
            if (_DirectionDelta > (0 - transform.eulerAngles.y))
            {
                _VectorDirection = 0 - _CollisionDelta - transform.eulerAngles.y;
            }
            else if (_DirectionDelta < (0 - transform.eulerAngles.y))
            {
                _VectorDirection = _CollisionDelta - transform.eulerAngles.y;
            }
            _DirectionDelta = 0 - transform.eulerAngles.y;
        }
    }
    private void OnTriggerEnter(Collider triggerEnter)
    {
        if (triggerEnter.gameObject.name == "FinishLine")
        {
            wincontroller.GameWin();
        }
    }
}
