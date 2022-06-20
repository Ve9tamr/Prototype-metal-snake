using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerMover : MonoBehaviour
{
    public Transform SP1;
    public Transform SP2;
    public Transform MainLine;
    public int LineType;
    private Vector3 moveforward;
    private Vector3 moveleft;
    public GameObject BlockingRock;

    private void Awake()
    {
        moveforward = Vector3.forward * 100;
        moveleft = Vector3.left * 90;
    }
    public void MoveSpawners()
    {
        MainLine.position += moveforward;
        SP1.position += moveforward;
        SP2.position += moveforward;
        if (LineType == 2)
        {
            MainLine.position += moveleft;
            SP1.position += moveleft;
            SP2.position += moveleft;
            BlockingRock.SetActive(true);
            Destroy(gameObject);
        }
        else if (LineType == 3)
        {
            MainLine.position -= moveleft;
            SP1.position -= moveleft;
            SP2.position -= moveleft;
            BlockingRock.SetActive(true);
            Destroy(gameObject);
        }
    }
}
