using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject SpawnerStart;
    private EnemySpawn MakeSpawn;
    public Transform BSpawner;
    public Transform EnemyFolder;
    public GameObject BossPrefab;

    private void Awake()
    {
        MakeSpawn = SpawnerStart.GetComponent<EnemySpawn>();
    }
    public void SpawnBoss()
    {
        Instantiate(BossPrefab, BSpawner.position, Quaternion.identity, EnemyFolder);
        MakeSpawn.SpawnWave();
        Destroy(gameObject);
    }
}
