using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovment : MonoBehaviour
{
    public int _CurState = 2;
    [SerializeField] private float _StateTimer = 0f;
    public float _StateDelta;
    public GameObject PlayerTruck;
    private TruckMovement TruckScript;
    [SerializeField] private float PlayerDistance;
    public float PursuitDistance;
    public float EvasionDistance;
    [SerializeField] private Vector3 _pursuitDirection;
    [SerializeField] private float XDelta;
    [SerializeField] private float ZDelta;

    public float _MoveSpeed;
    public float _MaxSpeed;
    public float _FulSpeed;
    [SerializeField] private float _deltaSpeed;
    public float _MinAcseleration;
    public float _PercentAcseleration;

    public float _VectorDirection;
    [SerializeField] private float _CurrentDirection;
    public float _RotationSpeed;

    private void Start()
    {
        TruckScript = PlayerTruck.GetComponent<TruckMovement>();
    }
    private void Update()
    {
        _StateTimer += Time.deltaTime;
        if (_StateTimer > _StateDelta)
        {
            _StateTimer = 0f;
            ChooseStait();
        }
        MoveEnemy();
        if (Mathf.Abs(_VectorDirection - _CurrentDirection) > 1)
        {
            RotationEnemy();
        }

    }
    private void ChooseStait()
    {
        PlayerDistance = Vector3.Magnitude(transform.position - PlayerTruck.transform.position);
        if (PlayerDistance > PursuitDistance)
        {
            _CurState = 2;
            Pursuit();
        }
        else if (PlayerDistance > EvasionDistance)
        {
            _CurState = 3;
            NormalMove();
        }
        else
        {
            _CurState = 4;
            Evasion();
        }
    }
    private void MoveEnemy()
    {
        if (_MoveSpeed < _MaxSpeed)
        {
            _deltaSpeed = _MaxSpeed - _MoveSpeed;
            _MoveSpeed += (_deltaSpeed * _PercentAcseleration + _MinAcseleration) * Time.deltaTime;
        }
        else if (_MoveSpeed > _MaxSpeed * 1.02)
        {
            _MoveSpeed -= (_MaxSpeed * _PercentAcseleration + _MinAcseleration) * Time.deltaTime;
        }
        transform.Translate(_MoveSpeed * Time.deltaTime * Vector3.forward);
    }
    private void RotationEnemy()
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
    private void Pursuit()
    {
        _MaxSpeed = _FulSpeed;
        XDelta = Mathf.Abs(transform.position.x - PlayerTruck.transform.position.x);
        if (XDelta < EvasionDistance)
        {
            ZDelta = transform.position.z - PlayerTruck.transform.position.z;
            if (ZDelta < 0)
            {
                _VectorDirection = 0;
            }
            else
            {
                _VectorDirection = 180;
            }
        }
        else
        {
            _pursuitDirection = (PlayerTruck.transform.position + 20f * PlayerTruck.transform.forward) - transform.position;
            if (Vector3.Angle(_pursuitDirection, transform.forward) > 5f)
            {
                _VectorDirection = -Vector3.SignedAngle(Vector3.forward, _pursuitDirection, Vector3.up);
            }
            else
            {
                _VectorDirection = _CurrentDirection;
            }
        }
    }
    private void NormalMove()
    {
        _MaxSpeed = TruckScript._MoveSpeed;
        _VectorDirection = TruckScript._CurrentDirection;
    }
    private void Evasion()
    {
        _MaxSpeed = _FulSpeed;
        XDelta = transform.position.x - PlayerTruck.transform.position.x;
        if (XDelta > 0)
        {
            _VectorDirection = -90;
        }
        else
        {
            _VectorDirection = 90;
        }
    }
}