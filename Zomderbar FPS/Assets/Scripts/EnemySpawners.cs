using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawners : MonoBehaviour
{
    [SerializeField] GameObject Door;
    [SerializeField] GameObject[] spawners;
    [SerializeField] GameObject[] enemies;
    [SerializeField] public int enemiesToSpawn;
    [SerializeField] float spawnInterval;
    [SerializeField] bool bossToSpawn;
    [Range(1, 10)] [SerializeField] int waves;
    public int enemiesSpawned;
    public bool spawnerActivated = false;
    private GameObject boss;
    private enemyAI bossScript;
    private bool bossSpawned;

    private void Awake()
    {
        if (bossToSpawn)
        {
            boss = GameObject.FindGameObjectWithTag("Boss");
            bossScript = boss.GetComponent<enemyAI>();
            boss.SetActive(false);
        }

    }
    private void Update()
    {
        if (bossToSpawn && gameManager.instance.enemyKilled == enemiesToSpawn * waves && spawnerActivated)
        {
            boss.SetActive(true);
            bossSpawned = true;

        }

        if (bossSpawned && bossScript.HP <= 0)
            Door.SetActive(false);
            

    }
    private void OnTriggerEnter(Collider other)
    {


        if (other.tag == "Player" && !spawnerActivated)
        {
            spawnerActivated = true;
            StartCoroutine(SpawnEnemy());
        }
    }

    private IEnumerator SpawnEnemy()
    {
        for (int i = 0; i < waves; ++i)
        {
            while (enemiesSpawned < enemiesToSpawn)
            {
                int enemy = Random.Range(0, enemies.Length);
                int spawn = Random.Range(0, spawners.Length);
                Instantiate(enemies[enemy], spawners[spawn].transform.position, Quaternion.identity);
                yield return new WaitForSeconds(spawnInterval);
                enemiesSpawned++;
            }
            enemiesSpawned = 0;


        }



        //for (int i = 0; i < enemiesToSpawn; i++)
        //{
        //    for (int enemy = 0; enemy < enemies.Length; enemy++)
        //    {
        //        for (int spawn = 0; spawn < spawners.Length; spawn++)
        //        {

        //            Instantiate(enemies[enemy], spawners[spawn].transform.position, Quaternion.identity);
        //            yield return new WaitForSeconds(spawnInterval);
        //        }
        //    }
        //}
    }
}
