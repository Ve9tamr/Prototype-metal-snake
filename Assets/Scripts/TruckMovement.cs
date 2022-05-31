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
    public Transform connectedtrailers;

    public float maxHP;
    public float curHP;
    public GameObject TailtoDestroy;
    public Text tailsnumber;
    public Image hpBar;

    private void Awake()
    {
        wincontroller = canvaspad.GetComponent<MenuUIController>();
        curHP = maxHP;
        TailsChange();
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
        else if (_MoveSpeed > _MaxSpeed * 1.02)
        {
            _MoveSpeed -= (_MaxSpeed * _PercentAcseleration + _MinAcseleration) * Time.deltaTime;
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
                Cargo.transform.rotation = Quaternion.Euler(0f, ThisRotation, 0f);
            }
            PreviousRotation = ThisRotation;
            ConnectorPoint = Cargo.position - (Cargo.forward * 5f);
        }
    }
    private void OnCollisionEnter(Collision collisionEnter)
    {
        if (collisionEnter.gameObject.CompareTag("Obstacle") || collisionEnter.gameObject.CompareTag("Enemy"))
        {
            _DirectionDelta = 0 - transform.eulerAngles.y;
            if (collisionEnter.gameObject.CompareTag("Obstacle"))
            {
                curHP -= _MoveSpeed * (_MoveSpeed + 5f) - 50;
                _MoveSpeed = 5f;
            }
            else
            {
                curHP -= _MoveSpeed * (_MoveSpeed * 0.5f - 5f) + 50;
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
    private void OnTriggerEnter(Collider triggerEnter)
    {
        if (triggerEnter.gameObject.CompareTag("Finish"))
        {
            wincontroller.GameWin();
        }
        if (triggerEnter.gameObject.CompareTag("Food"))
        {
            Destroy(triggerEnter.gameObject);
            var Cargo = Instantiate(TailPrefab, connectedtrailers);
            Tails.Add(Cargo.transform);
            TailsChange();
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
        if (Tails.Count > 2)
        {
            _MaxSpeed = _FulSpeed * Mathf.Pow(0.9f, Tails.Count - 2);
        }
        else
        {
            _MaxSpeed = _FulSpeed;
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
}
