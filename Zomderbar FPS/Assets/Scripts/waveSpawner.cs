using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveSpawner : MonoBehaviour
{
    [SerializeField] int totalWaves;
    [SerializeField] GameObject[] enemies;
    [SerializeField] int spawnInterval;
    [SerializeField] int enemiesToSpawn;

    public Transform[] spawnLocation;

    public int enemiesSpawned;
    public int currWave = 1;
    bool activateSpanwer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            activateSpanwer = true;
        }
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (activateSpanwer)
        {
            while (currWave < totalWaves && activateSpanwer)
            {
                SpawnWave();
            }
        }
    }

    IEnumerator SpawnWave()
    {
       
        while (enemiesSpawned < enemiesToSpawn)
        {
            int enemy = Random.Range(0, enemies.Length);
            int spawn = Random.Range(0, spawnLocation.Length);
            Instantiate(enemies[enemy], spawnLocation[spawn].transform.position, Quaternion.identity);
            enemiesSpawned++;
            yield return new WaitForSeconds(spawnInterval);
        }

    }


}
