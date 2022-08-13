using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SelectFire
{
    Full,
    Semi
}

//public enum SelectedWeapon { AR = 1, SMG, Shotgun, Sniper, Pistol, Knife };

[CreateAssetMenu]
public class gunStats : ScriptableObject
{
    public SelectFire fireType = SelectFire.Full;

    [Header("General")]
    public float shootRate;
    public float shootDist;
    public int shootDmg;
    public GameObject gunHUD;
    //public SelectedWeapon selectedWeapon;

    [Header("Ammo")]
    public int ammoCapacity;
    private int currentAmmo;
    public float reloadTime;
}
