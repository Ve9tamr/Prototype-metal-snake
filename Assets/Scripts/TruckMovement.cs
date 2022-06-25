using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TruckMovement : MonoBehaviour
{
    public float _MoveSpeed;
    public float _MaxSpeed;
    public float _FulSpeed;
    private float _deltaSpeed;
    public float _MinAcseleration;
    public float _PercentAcseleration;

    public float _VectorDirection;
    public float _CurrentDirection;
    public float _MaxRotSpeed;
    public float _RotationSpeed;
    private float _DirectionDelta;
    public float _CollisionDelta;
    private int _CollisionFactor;

    public GameObject canvaspad;
    private MenuUIController wincontroller;

    private Vector3 ConnectorPoint;
    private float PreviousRotation;
    private float DeltaRotation;
    private float ThisRotation;
    public List<Transform> Tails;
    public GameObject TailPrefab;
    public Transform connectedtrailers;

    public float maxHP;
    public float curHP;
    public GameObject TailtoDestroy;
    public Text tailsnumber;
    public Image hpBar;
    public AudioSource TruckMv;

    private void Awake()
    {
        wincontroller = canvaspad.GetComponent<MenuUIController>();
        curHP = maxHP;
        _CollisionFactor = 1;
        _MoveSpeed = 5;
        TailsChange();
    }
    void Update()
    {
        if (Mathf.Abs(_VectorDirection - _CurrentDirection) > 3)
        {
            TruckRotation();
        }
        MoveTruck();
        ConnectorPoint = transform.position - (transform.forward * 3f);
        PreviousRotation = transform.eulerAngles.y;
        MoveTails();
        TruckMv.volume = (_MoveSpeed + 60) / 400;
    }
    private void MoveTruck()
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
            _CurrentDirection += _RotationSpeed * Time.deltaTime * _CollisionFactor;
        }
        else
        {
            _CurrentDirection -= _RotationSpeed * Time.deltaTime * _CollisionFactor;
        }
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
        if (collisionEnter.gameObject.CompareTag("Obstacle") || collisionEnter.gameObject.CompareTag("Enemy"))
        {
            _CollisionFactor = 2;
            _DirectionDelta = 0 - transform.eulerAngles.y;
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
    private void OnCollisionStay(Collision collisionStay)
    {
        if (collisionStay.gameObject.CompareTag("Obstacle") || collisionStay.gameObject.CompareTag("Enemy"))
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
        if (collisionExit.gameObject.CompareTag("Obstacle") || collisionExit.gameObject.CompareTag("Enemy"))
        {
            _CollisionFactor = 1;
        }
    }
    private void OnTriggerEnter(Collider triggerEnter)
    {
        if (triggerEnter.gameObject.CompareTag("Food"))
        {
            Destroy(triggerEnter.gameObject);
            var Cargo = Instantiate(TailPrefab, connectedtrailers);
            Tails.Add(Cargo.transform);
            ++wincontroller.TrailersGained;
            TailsChange();
        }
        else if (triggerEnter.gameObject.name == "SpawnerLine")
        {
            SpawnerMover SMover = triggerEnter.gameObject.GetComponent<SpawnerMover>();
            SMover.MoveSpawners();
        }
        else if (triggerEnter.gameObject.name == "FirstEncounter")
        {
            FirstSpawn FSpawn = triggerEnter.gameObject.GetComponent<FirstSpawn>();
            FSpawn.MFSpawn();
        }
        else if (triggerEnter.gameObject.CompareTag("Finish"))
        {
            wincontroller.GameWin();
        }
    }
    private void TailsChange()
    {
        if (Tails.Count > 0)
        {
            TailtoDestroy = Tails[^1].gameObject;
            tailsnumber.color = Color.black;
        }
        else
        {
            tailsnumber.color = Color.red;
        }
        tailsnumber.text = Tails.Count.ToString();
        wincontroller.TrailersFinished = Tails.Count;
        if (Tails.Count > 2)
        {
            _MaxSpeed = _FulSpeed * Mathf.Pow(0.85f, Tails.Count - 2);
            _RotationSpeed = _MaxRotSpeed * Mathf.Pow(0.85f, Tails.Count - 2);
        }
        else
        {
            _MaxSpeed = _FulSpeed;
            _RotationSpeed = _MaxRotSpeed;
        }

    }
    private void CheckHP()
    {
        if (curHP <= 0)
        {
            if (Tails.Count == 0)
            {
                wincontroller.GameLose();
            }
            else
            {
                curHP = maxHP;
                Tails.RemoveAt(Tails.Count - 1);
                Destroy(TailtoDestroy);
                TailsChange();
            }
        }
        hpBar.fillAmount = curHP / maxHP;
    }
    public void BulletHit()
    {
        curHP -= 5f;
        CheckHP();
    }
}
