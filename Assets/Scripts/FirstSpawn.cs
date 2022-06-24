using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstSpawn : MonoBehaviour
{
    public GameObject SpawnMaster;
    private EnemySpawn makespawn;
    public GameObject MainUI;
    private MenuUIController MainUIControl;

    private void Awake()
    {
        makespawn = SpawnMaster.GetComponent<EnemySpawn>();
        MainUIControl = MainUI.GetComponent<MenuUIController>();
    }
    public void MFSpawn()
    {
        makespawn.EncounterAchived();
        Destroy(gameObject);
        MainUIControl.Tutorial3();
    }
}
