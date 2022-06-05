using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstSpawn : MonoBehaviour
{
    public GameObject SpawnMaster;
    private EnemySpawn makespawn;

    private void Awake()
    {
        makespawn = SpawnMaster.GetComponent<EnemySpawn>();
    }
    public void MFSpawn()
    {
        makespawn.EncounterAchived();
        Destroy(gameObject);
    }
}
