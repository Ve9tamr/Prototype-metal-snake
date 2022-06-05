using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerMover : MonoBehaviour
{
    public Transform SP1;
    public Transform SP2;
    private Vector3 moveforward;

    private void Awake()
    {
        moveforward = Vector3.forward * 100;
    }
    public void MoveSpawners()
    {
        transform.position += moveforward;
        SP1.position += moveforward;
        SP2.position += moveforward;
    }
}
