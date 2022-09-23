using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieRockBreakerSpawner : MonoBehaviour
{
    [SerializeField] Transform SpawnPosition;
    [SerializeField] GameObject prefabZombie;
    GameObject[] enemies;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if(enemies.Length < 1)
        {
            Instantiate(prefabZombie, SpawnPosition.position, Quaternion.identity);
        }
    }
}
