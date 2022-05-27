using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject TruckPosition;
    public Vector3 _difToCamera;
    private Vector3 _NewCameraPosition;
    private Vector3 _CameraFollowPosition;
    private Vector3 _MoveDirection;
    private float _MoveDistance;
    public float CameraMoveSpeed;

    void Awake()
    {
        transform.position = TruckPosition.transform.position + _difToCamera;
    }

    void Update()
    {
        _CameraFollowPosition = TruckPosition.transform.position + _difToCamera;
        _MoveDirection = (_CameraFollowPosition - transform.position).normalized;
        _MoveDistance = Vector3.Distance(_CameraFollowPosition, transform.position);
        if (_MoveDistance > 0.1f)
        {
            _NewCameraPosition = transform.position +  _MoveDistance * CameraMoveSpeed * Time.deltaTime * _MoveDirection;
            float distanceAfterMoving = Vector3.Distance(_NewCameraPosition, _CameraFollowPosition);
            if (distanceAfterMoving > _MoveDistance)
            {
                _NewCameraPosition = _CameraFollowPosition;
            }
            transform.position = _NewCameraPosition;
        }
    }
}
