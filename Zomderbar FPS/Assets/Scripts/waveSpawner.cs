using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveSpawner : MonoBehaviour
{
    [SerializeField] int totalWaves;
    [SerializeField] GameObject[] enemies;
    [SerializeField] int spawnInterval;
    [SerializeField] int enemiesInWave;
    [SerializeField] List<GameObject> enemiesToSpawn;

    public GameObject[] spawnLocation;

    public int enemiesSpawned;
    public int currWave;
    public bool activateSpanwer = false;
    private bool firstSpawn = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !firstSpawn)
        {
            activateSpanwer = true;
            firstSpawn = true;
        }
    }



    private void Update()
    {
        if (firstSpawn && !activateSpanwer && gameManager.instance.enemyKilled == enemiesInWave)
        {
            activateSpanwer = true;
            gameManager.instance.enemyKilled = 0;
            ++currWave;

        }

        if (activateSpanwer && currWave <= totalWaves)
        {
            
            activateSpanwer = false;
            StartCoroutine(SpawnWave());

            
            
            

        }
    }

    void GenerateEnemies()
    {
        enemiesToSpawn.Clear();
        for (int i = 0; i < enemiesInWave; ++i)
        {
            
            int enemy = Random.Range(0, enemies.Length);
            enemiesToSpawn.Add(enemies[enemy]);
        }
    }


    IEnumerator SpawnWave()
    {
        GenerateEnemies();
        for (int i = 0; i < enemiesToSpawn.Count; ++i)
        {

            int spawnLoc = Random.Range(0, spawnLocation.Length);
            Instantiate(enemiesToSpawn[i], spawnLocation[spawnLoc].transform.position, Quaternion.identity);
            enemiesSpawned++;
            gameManager.instance.enemyCount++;
            yield return new WaitForSeconds(spawnInterval);


        }

    }


}
