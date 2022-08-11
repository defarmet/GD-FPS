using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SelectFire
{
    Full,
    Semi
}

[CreateAssetMenu]
public class gunStats : ScriptableObject
{
    public SelectFire fireType = SelectFire.Full;

    [Header("General")]
    public float shootRate;
    public int shootDist;
    public int shootDmg;

    [Header("Ammo")]
    public int ammoCapacity = 18;
    private int currentAmmo;
    public float reloadTime = 2;
}
