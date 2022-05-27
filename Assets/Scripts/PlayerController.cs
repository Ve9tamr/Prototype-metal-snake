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
    public GameObject TruckBody;
    private TruckMovement rotupdate;

    private void Awake()
    {
        _PointDirection = Vector2.up;
        _BasicDirection = Vector2.up;
        _PointSnake.Set(540.0f, 940.0f);
        rotupdate = TruckBody.GetComponent<TruckMovement>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _PointTouched = eventData.position;
        Debug.Log(_PointTouched);

        if (Vector2.Distance(_PointTouched, _PointSnake)>150f)
        {
            _PointDirection = _PointTouched - _PointSnake;
            rotupdate._VectorDirection = Vector2.SignedAngle(_BasicDirection, _PointDirection);
        }
    }
}
