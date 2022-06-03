using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckShooting : MonoBehaviour
{
    public GameObject Aim;
    public float _AimRotation;
    public float _GunRotation;
    public float _RotationSpeed;
    public Vector3 _AimDirection;

    public GameObject Truck;
    private TruckMovement BodyRotation;

    private void Awake()
    {
        BodyRotation = Truck.GetComponent<TruckMovement>();
    }
    private void Update()
    {
        if (Aim == null)
        {
            _AimRotation = 0f;
        }
        else
        {
            _AimDirection = Aim.transform.position - transform.position;
            _AimDirection.y = 0f;
            _AimRotation = - Vector3.SignedAngle(Truck.transform.forward, _AimDirection, Vector3.up);
        }
        if (Mathf.Abs(_AimRotation - _GunRotation) > 3)
        {
            GunRotation();
        }
    }
    private void GunRotation()
    {
        if (_AimRotation - 180 > _GunRotation)
        {
            _GunRotation += 360;
        }
        else if (_AimRotation + 180 < _GunRotation)
        {
            _GunRotation -= 360;
        }
        if (_AimRotation > _GunRotation)
        {
            _GunRotation += _RotationSpeed * Time.deltaTime;
        }
        else
        {
            _GunRotation -= _RotationSpeed * Time.deltaTime;
        }
        transform.rotation = Quaternion.Euler(0f, - BodyRotation._CurrentDirection - _GunRotation, 0f);
    }
}
