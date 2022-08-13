using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunPickup : MonoBehaviour
{
    [SerializeField] gunStats gunStat;
    [SerializeField] GameObject currentGunHUD;

    private void Update() // double pickup due to update
    {
        if (Input.GetButtonDown("Interact"))
        {
            gameManager.instance.playerScript.gunPickup(gunStat.shootRate, gunStat.shootDist, gunStat.shootDmg, gunStat.ammoCapacity, currentGunHUD, gunStat);
            Destroy(gameObject);
        }
    }
}
