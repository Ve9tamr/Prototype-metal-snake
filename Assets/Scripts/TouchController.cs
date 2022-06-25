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
    private bool CorrectTouch;

    public GameObject ArrowSprite;
    private ArrowDissolve ArrowScript;

    private void Awake()
    {
        _PointDirection = Vector2.up;
        _BasicDirection = Vector2.up;
        _PointSnake.Set(540.0f, 940.0f);
        rotupdate = TruckBody.GetComponent<TruckMovement>();
        ArrowScript = ArrowSprite.GetComponent<ArrowDissolve>();
    }
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Touch touch = Input.GetTouch(0);
            _PointTouched = touch.position;
            if (Vector2.Distance(_PointTouched, _PointSnake) > 150f && !EventSystem.current.IsPointerOverGameObject(0))
            {
                CorrectTouch = true;
                _PointDirection = _PointTouched - _PointSnake;
                rotupdate._VectorDirection = Vector2.SignedAngle(_BasicDirection, _PointDirection);
                ArrowSprite.transform.position = touch.position;
                ArrowScript.transform.rotation = Quaternion.Euler(0f, 0f, rotupdate._VectorDirection);
                ArrowScript.NewPlacing();
            }
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && CorrectTouch)
        {
            Touch touch = Input.GetTouch(0);
            _PointTouched = touch.position;
            _PointDirection = _PointTouched - _PointSnake;
            rotupdate._VectorDirection = Vector2.SignedAngle(_BasicDirection, _PointDirection);
            ArrowSprite.transform.position = touch.position;
            ArrowScript.transform.rotation = Quaternion.Euler(0f, 0f, rotupdate._VectorDirection);
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            if (!ArrowScript.Started)
            {
                ArrowScript.StartDissolvation();
                    CorrectTouch = false;
            }
        }
    }
}
