using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//public enum SelectFire
//{
//    Full,
//    Semi
//}

[CreateAssetMenu]
public class gunStats : ScriptableObject
{
    //public SelectFire fireType = SelectFire.Full;

    [Header("General")]
    public float shootRate;
    public float shootDist;
    public int shootDmg;
    public GameObject model;

    //[Header("Ammo")]
    //public int ammoCapacity = 18;
    //private int currentAmmo;
    //public float reloadTime = 2;
}
