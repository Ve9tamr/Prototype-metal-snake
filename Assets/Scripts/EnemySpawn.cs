using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public bool FirstEncounter;
    public GameObject CurrentSpawnerA;
    public GameObject CurrentSpawnerB;
    public Transform CurrentSpawner;
    public float GlobalTimer;
    public float NextWave;
    private float SpawnerTimer;
    private int SpawnCounter;
    private int LateSpawnCounter;
    private float RandomEnemy;

    public GameObject EnemyPrefab;
    public GameObject RamPrefab;
    public Transform EnemyFolder;

    private void Awake()
    {
        GlobalTimer = 0;
        LateSpawnCounter = 0;
        FirstEncounter = false;
        SpawnerTimer = 0;
    }
    private void Update()
    {
        if (SpawnerTimer > 0)
        {
            SpawnerTimer -= Time.deltaTime;
            if (SpawnerTimer <= 0)
            {
                SpawnEnemy();
            }
        }
        GlobalTimer += Time.deltaTime;
        if (FirstEncounter && GlobalTimer > NextWave)
        {
            SpawnWave();
        }
    }
    public void EncounterAchived()
    {
        FirstEncounter = true;
        SpawnWave();
    }
    private void GenerateWaveTimer()
    {
        if (GlobalTimer < 90f)
        {
            NextWave = GlobalTimer + Random.Range(15f, 20f);
        }
        else
        {
            NextWave = GlobalTimer + Random.Range(5f, 10f);
        }
    }
    public void SpawnWave()
    {
        if (GlobalTimer < 90f)
        {
            SpawnCounter = 2;
        }
        else
        {
            SpawnCounter = ++LateSpawnCounter;
        }
        GenerateWaveTimer();
        SpawnEnemy();
    }
    public void SpawnEnemy()
    {
        RandomEnemy = Random.Range(0f, 100f);
        if (SpawnCounter % 2 == 0)
        {
            CurrentSpawner = CurrentSpawnerA.GetComponent<Transform>();
        }
        else
        {
            CurrentSpawner = CurrentSpawnerB.GetComponent<Transform>();
        }
        if (RandomEnemy > 40)
        {
            Instantiate(EnemyPrefab, CurrentSpawner.position, Quaternion.identity, EnemyFolder);
        }
        else
        {
            Instantiate(RamPrefab, CurrentSpawner.position, Quaternion.identity, EnemyFolder);
        }
        if (--SpawnCounter > 0)
        {
            SpawnerTimer = Random.Range(0.5f, 1f);
        }
    }
}
