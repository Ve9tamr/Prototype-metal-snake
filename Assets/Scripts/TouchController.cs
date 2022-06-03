using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchController : MonoBehaviour
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
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Touch touch = Input.GetTouch(0);
            _PointTouched = touch.position;
            if (Vector2.Distance(_PointTouched, _PointSnake) > 150f && !EventSystem.current.IsPointerOverGameObject(0))
            {
                _PointDirection = _PointTouched - _PointSnake;
                rotupdate._VectorDirection = Vector2.SignedAngle(_BasicDirection, _PointDirection);
            }
        }
    }
}
