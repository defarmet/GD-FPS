using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toggleEnemy : MonoBehaviour
{
    [SerializeField]GameObject[] enemies;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            foreach (var x in enemies)
                x.SetActive(true);
        }
        Destroy(gameObject);
    }

    
}
