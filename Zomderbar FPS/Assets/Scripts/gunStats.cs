using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class gunStats : ScriptableObject
{
    [Header("General")]
    public float      shootRate;
    public float      shootDist;
    public int        shootDmg;
    public int        gunHUD;
    public GameObject model;
    public AudioClip  shootSound;
    public AudioClip  reloadSound;

    [Header("Ammo")]
    public  int   ammoCapacity;
            int   currentAmmo;
    public  float reloadTime;
}
