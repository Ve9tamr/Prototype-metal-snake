using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour, IPointerDownHandler
{
    private Vector2 _PointTouched;
    private Vector2 _PointSnake;
    private Vector2 _PointDirection;
    private Vector2 _BasicDirection;
    private float _VectorDirection;
    private float _CurrentDirection;
    public float _RotationSpeed;
    public GameObject TruckBody;

    private void Awake()
    {
        _PointDirection = Vector2.up;
        _BasicDirection = Vector2.up;
        _PointSnake = new Vector2(540.0f, 920.0f);
    }

    private void Update()
    {
        if (Mathf.Abs(_VectorDirection - _CurrentDirection) > 1)
        {
            TruckRotation();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _PointTouched = eventData.position;

        if (Vector2.Distance(_PointTouched, _PointSnake)>150f)
        {
            _PointDirection = _PointTouched - _PointSnake;
            _VectorDirection = Vector2.SignedAngle(_BasicDirection, _PointDirection);
        }
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

        TruckBody.transform.rotation = Quaternion.Euler(0f, -_CurrentDirection, 0f);
    }
}
