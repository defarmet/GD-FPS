using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//public enum SelectedWeapon { AR = 1, SMG, Shotgun, Sniper, Pistol, Knife };

[CreateAssetMenu]
public class gunStats : ScriptableObject
{

    [Header("General")]
    public float shootRate;
    public float shootDist;
    public int shootDmg;
    public int gunHUD;
    //public SelectedWeapon selectedWeapon;

    [Header("Ammo")]
    public int ammoCapacity;
    private int currentAmmo;
    public float reloadTime;
}
