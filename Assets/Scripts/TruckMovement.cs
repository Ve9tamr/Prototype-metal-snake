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
}
