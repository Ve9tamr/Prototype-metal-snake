using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimOn : MonoBehaviour
{
    public GameObject PlayerWeapon;
    public GameObject AimImage;
    public GameObject[] AllEnemies;
    private TruckShooting AimScript;

    private void Awake()
    {
        if (PlayerWeapon == null)
        {
            PlayerWeapon = GameObject.FindWithTag("PlayerWeapon");
        }
        AimScript = PlayerWeapon.GetComponent<TruckShooting>();
    }
    public void AimMe()
    {
        AllEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject MyEnemy in AllEnemies)
        {
            AimOn NoAim = MyEnemy.GetComponent<AimOn>();
            NoAim.AimRemove();
        }
        AimScript.Aim = gameObject;
        AimImage.SetActive(true);
    }
    public void AimRemove()
    {
        AimImage.SetActive(false);
    }
}
