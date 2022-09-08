using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorDrop : MonoBehaviour
{
    [SerializeField] int enemiesToKill;
    
    void Update()
    {
        if (enemiesToKill <= gameManager.instance.enemyKilled)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }
}
