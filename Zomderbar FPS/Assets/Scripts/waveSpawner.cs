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
    [SerializeField] bool bossToSpawn;
    [SerializeField] GameObject door;


    public GameObject[] spawnLocation;
    private GameObject boss;
    private enemyAI bossScript;
    public int enemiesSpawned;
    public int currWave;
    public bool activateSpawner = false;
    private bool firstSpawn = false;

    private void Awake()
    {
        if (bossToSpawn)
        {
            boss = GameObject.FindGameObjectWithTag("Boss");
            bossScript = boss.GetComponent<enemyAI>();
            boss.SetActive(false);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !firstSpawn)
        {
            activateSpawner = true;
            firstSpawn = true;
        }
    }



    private void Update()
    {
        if (firstSpawn && !activateSpawner && gameManager.instance.enemyKilled == enemiesInWave * currWave)
        {
            activateSpawner = true;
            ++currWave;

            if (currWave > totalWaves && bossToSpawn)
            {
                boss.SetActive(true);

            }

            
        }

        if (activateSpawner && currWave <= totalWaves)
        {

            activateSpawner = false;
            GenerateEnemies();
            StartCoroutine(SpawnWave());


        }

        if (bossToSpawn && bossScript.HP <= 0)
            door.SetActive(false);


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
