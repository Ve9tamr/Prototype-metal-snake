using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckMovement : MonoBehaviour
{
    public float _MoveSpeed;
    void Update()
    {
        transform.Translate(_MoveSpeed * Time.deltaTime * Vector3.right);
    }
}
