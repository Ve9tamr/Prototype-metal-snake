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
    public Vector3 BoxCenter;
    public Vector3 BoxSize;
    public Vector3 ForwardSize;

    public float _MoveSpeed;
    public float _MaxSpeed;
    public float _FulSpeed;
    [SerializeField] private float _deltaSpeed;
    public float _MinAcseleration;
    public float _PercentAcseleration;

    public float _VectorDirection;
    public float _CurrentDirection;
    public float _RotationSpeed;
    private float _DirectionDelta;
    public float _CollisionDelta;
    private int _CollisionFactor;

    public float maxHP;
    public float curHP;

    public AudioSource EnemyMv;

    private void Awake()
    {
        PlayerTruck = GameObject.FindWithTag("Player");
        TruckScript = PlayerTruck.GetComponent<TruckMovement>();
        _CollisionFactor = 1;
        curHP = maxHP;
    }
    private void Update()
    {
        _StateTimer += Time.deltaTime;
        if (_StateTimer > _StateDelta)
        {
            _StateTimer = 0f;
            ChooseStait();
        }
        if (Mathf.Abs(_VectorDirection - _CurrentDirection) > 3)
        {
            RotationEnemy();
        }
        MoveEnemy();
        EnemyMv.volume = (_MoveSpeed + 15) / 200;
    }
    private void ChooseStait()
    {
        if (transform.position.y < 0)
        {
            float makex = transform.position.x;
            float makez = transform.position.z;
            transform.position = new Vector3(makex, 0, makez);
        }
        if (Physics.BoxCast(transform.position + BoxCenter, BoxSize, transform.forward, Quaternion.Euler(transform.forward), 14f))
        {
            _CurState = 1;
            Jink();
        }
        else
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
        
    }
    private void MoveEnemy()
    {
        transform.rotation = Quaternion.Euler(0f, -_CurrentDirection, 0f);
        if (_MoveSpeed < _MaxSpeed)
        {
            _deltaSpeed = _MaxSpeed - _MoveSpeed;
            _MoveSpeed += (_deltaSpeed * _PercentAcseleration + _MinAcseleration) * Time.deltaTime;
        }
        else if (_MoveSpeed > _MaxSpeed * 1.02)
        {
            _MoveSpeed -= (_FulSpeed * _PercentAcseleration + _MinAcseleration) * Time.deltaTime;
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
            _CurrentDirection += _RotationSpeed * Time.deltaTime * _CollisionFactor;
        }
        else
        {
            _CurrentDirection -= _RotationSpeed * Time.deltaTime * _CollisionFactor;
        }
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
    private void Jink()
    {
        if (!Physics.BoxCast(transform.position + BoxCenter, ForwardSize, transform.forward, Quaternion.Euler(transform.forward), 14f))
        {
            _MaxSpeed = _FulSpeed;
            _VectorDirection = _CurrentDirection;
        }
        else if (!Physics.Raycast(transform.position + BoxCenter * 0.2f, transform.forward * 12 + transform.right * 7, 14) && 
            !Physics.Raycast(transform.position + BoxCenter * 0.2f, transform.forward * 12 + transform.right * 4, 13))
        {
            _MaxSpeed = _FulSpeed;
            _VectorDirection = _CurrentDirection - 45;
        }
        else if (!Physics.Raycast(transform.position + BoxCenter * 0.2f, transform.forward * 12 + transform.right * -7, 14) &&
            !Physics.Raycast(transform.position + BoxCenter * 0.2f, transform.forward * 12 + transform.right * -4, 13))
        {
            _MaxSpeed = _FulSpeed;
            _VectorDirection = _CurrentDirection + 45;
        }
        else if (!Physics.Raycast(transform.position + BoxCenter * 0.2f, transform.forward * 10 + transform.right * -6, 12) &&
            !Physics.Raycast(transform.position + BoxCenter * 0.2f, transform.forward * 8 + transform.right * -2, 8))
        {
            _MaxSpeed = 5f;
            _VectorDirection = _CurrentDirection + 90;
        }
        else if (!Physics.Raycast(transform.position + BoxCenter * 0.2f, transform.forward * 10 + transform.right * 6, 12) &&
            !Physics.Raycast(transform.position + BoxCenter * 0.2f, transform.forward * 8 + transform.right * 2, 8))
        {
            _MaxSpeed = 5f;
            _VectorDirection = _CurrentDirection - 90;
        }
        else
        {
            _MaxSpeed = 5f;
            if (_VectorDirection > 0)
            {
                _VectorDirection -= 180f;
            }
            else
            {
                _VectorDirection += 180f;
            }
        }
    }
    private void OnCollisionEnter(Collision collisionEnter)
    {
        if (collisionEnter.gameObject.CompareTag("Obstacle") || collisionEnter.gameObject.CompareTag("Player") || collisionEnter.gameObject.CompareTag("Enemy"))
        {
            _DirectionDelta = 0 - transform.eulerAngles.y;
            _CollisionFactor = 2;
            if (PlayerDistance < 70)
            {
                if (collisionEnter.gameObject.CompareTag("Obstacle"))
                {
                    curHP -= _MoveSpeed * (_MoveSpeed * 0.5f + 5f) - 35;
                    _MoveSpeed = 5f;
                }
                else
                {
                    curHP -= _MoveSpeed * (_MoveSpeed * 0.3f + 3f) + 10;
                }
                CheckHP();
            }
        }
    }
    private void OnCollisionStay(Collision collisionStay)
    {
        if (collisionStay.gameObject.CompareTag("Obstacle") || collisionStay.gameObject.CompareTag("Player") || collisionStay.gameObject.CompareTag("Enemy"))
        {
            _CurrentDirection = 0 - transform.eulerAngles.y;
            if (collisionStay.gameObject.CompareTag("Obstacle"))
            {
                _MoveSpeed = 5f;
            }
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
    private void OnCollisionExit(Collision collisionExit)
    {
        if (collisionExit.gameObject.CompareTag("Obstacle") || collisionExit.gameObject.CompareTag("Enemy") || collisionExit.gameObject.CompareTag("Player"))
        {
            _CollisionFactor = 1;
        }
    }
    private void CheckHP()
    {
        if (curHP <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void BulletHit()
    {
        curHP -= 25f;
        CheckHP();
    }
}
