using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject Aim;
    public float _AimRotation;
    public float _GunRotation;
    public float _RotationSpeed;
    public Vector3 _AimDirection;

    public GameObject Sprat;
    private EnemyMovment BodyRotation;

    public float _ReloadTimer;
    public float _AimDistance;
    public float _ShootDistance;
    public GameObject BulletPosition;
    public Transform BulletPrefab;
    public GameObject forbullets;
    public Transform bullets;
    public AudioSource EnemySh;

    private void Awake()
    {
        BodyRotation = Sprat.GetComponent<EnemyMovment>();
        Aim = GameObject.FindWithTag("Player");
        forbullets = GameObject.Find("Bullets");
        bullets = forbullets.GetComponent<Transform>();
    }
    private void Update()
    {
        _AimDirection = Aim.transform.position - transform.position;
        _AimDirection.y = 0f;
        _AimRotation = -Vector3.SignedAngle(Sprat.transform.forward, _AimDirection, Vector3.up);
        if (Mathf.Abs(_AimRotation - _GunRotation) > 4)
        {
            GunRotation();
        }
        if (_ReloadTimer < 0.1f)
        {
            _ReloadTimer += Time.deltaTime;
        }
        else
        {
            _AimDistance = Vector3.Magnitude(_AimDirection);
            if (_AimDistance < _ShootDistance)
            {
                GunShooting();
            }
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
        transform.rotation = Quaternion.Euler(0f, -BodyRotation._CurrentDirection - _GunRotation, 0f);
    }
    public void GunShooting()
    {
        Transform bulletTrasform = Instantiate(BulletPrefab, BulletPosition.transform.position, Quaternion.identity, bullets);
        bulletTrasform.GetComponent<Bullet>().Setup(_AimDirection, Aim, true);
        _ReloadTimer = 0f;
        EnemySh.Play();
    }
}
