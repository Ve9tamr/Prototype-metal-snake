using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _LTimer;
    private float _LifeTime = 1f;
    public float _BulletSpeed;
    private Vector3 _ShootDir;
    private EnemyMovment AimHit;

    private void Update()
    {
        if (_LTimer < _LifeTime)
        {
            _LTimer += Time.deltaTime;
            transform.position += _BulletSpeed * Time.deltaTime * _ShootDir;
        }
        else
        {
            if (AimHit != null)
            {
                AimHit.BulletHit();
            }
            Destroy(gameObject);
        }
    }
    public void Setup(Vector3 _AimDir, GameObject Aim)
    {
        _ShootDir = (_AimDir).normalized;
        _LifeTime = (Vector3.Magnitude(_AimDir) - 3f) / _BulletSpeed;
        AimHit = Aim.GetComponent<EnemyMovment>();
        transform.rotation = Quaternion.Euler(_AimDir);
    }
}
