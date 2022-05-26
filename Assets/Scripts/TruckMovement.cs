using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckMovement : MonoBehaviour
{
    public float _MoveSpeed;
    public float _MaxSpeed;
    public float _deltaSpeed;
    public float _MinAcseleration;
    public float _PercentAcseleration;

    public GameObject touchpad;
    public PlayerController rotcontroller;
    private float _DirectionDelta;
    public float _CollisionDelta;
    private void Awake()
    {
        rotcontroller = touchpad.GetComponent<PlayerController>();
    }
    void Update()
    {
        if (_MoveSpeed < _MaxSpeed)
        {
            _deltaSpeed = _MaxSpeed - _MoveSpeed;
            _MoveSpeed += (_deltaSpeed * _PercentAcseleration + _MinAcseleration) * Time.deltaTime;
        }
        if (_MoveSpeed > _MaxSpeed * 1.05)
        {
            _MoveSpeed = _MaxSpeed;
        }
        transform.Translate(_MoveSpeed * Time.deltaTime * Vector3.forward);
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
            rotcontroller._CurrentDirection = 0 - transform.eulerAngles.y;
            _MoveSpeed = 5f;
            if (_DirectionDelta > (0 - transform.eulerAngles.y))
            {
                rotcontroller._VectorDirection = 0 - _CollisionDelta - transform.eulerAngles.y;
            }
            else if (_DirectionDelta < (0 - transform.eulerAngles.y))
            {
                rotcontroller._VectorDirection = _CollisionDelta - transform.eulerAngles.y;
            }
            _DirectionDelta = 0 - transform.eulerAngles.y;
        }
    }
}
