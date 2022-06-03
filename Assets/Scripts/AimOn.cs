using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimOn : MonoBehaviour
{
    public GameObject PlayerWeapon;
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
        AimScript.Aim = gameObject;
    }
}
