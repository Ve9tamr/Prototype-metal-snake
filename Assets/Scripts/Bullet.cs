using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _LTimer;
    private float _LifeTime = 1f;
    public float _BulletSpeed;
    private Vector3 _ShootDir;
    private EnemyMovment EnemyHit;
    private TruckMovement PlayerHit;

    private void Update()
    {
        if (_LTimer < _LifeTime)
        {
            _LTimer += Time.deltaTime;
            transform.position += _BulletSpeed * Time.deltaTime * _ShootDir;
        }
        else
        {
            if (EnemyHit != null)
            {
                EnemyHit.BulletHit();
            }
            else if (PlayerHit != null)
            {
                PlayerHit.BulletHit();
            }
            Destroy(gameObject);
        }
    }
    public void Setup(Vector3 _AimDir, GameObject Aim, bool AimIsPlayer)
    {
        _ShootDir = (_AimDir).normalized;
        _LifeTime = (Vector3.Magnitude(_AimDir) - 5f) / _BulletSpeed;
        if (_LifeTime > 1f)
        {
            _LifeTime = 1f;
        }
        if (AimIsPlayer)
        {
            PlayerHit = Aim.GetComponent<TruckMovement>();
        }
        else
        {
            EnemyHit = Aim.GetComponent<EnemyMovment>();
        }
        transform.rotation = Quaternion.Euler(0, Vector3.SignedAngle(Vector3.forward, _AimDir, Vector3.up), 0);
    }
}
