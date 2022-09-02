using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthPickup : MonoBehaviour
{
    [SerializeField] int gainHealth;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameManager.instance.playerScript.hp + gainHealth > gameManager.instance.playerScript.hpOriginal)
            {
                gameManager.instance.playerScript.hp = gameManager.instance.playerScript.hpOriginal;
            }
            else
            {
                gameManager.instance.playerScript.hp += gainHealth;
            }
  
            Destroy(gameObject);
        }
    }
}
