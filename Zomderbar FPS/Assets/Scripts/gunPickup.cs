using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunPickup : MonoBehaviour
{
    [SerializeField] gunStats gunStat;

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            gameManager.instance.playerScript.gunPickup(gunStat.shootRate, gunStat.shootDist, gunStat.shootDmg, gunStat);
            //Destroy(gameObject);
        }
    }
}
