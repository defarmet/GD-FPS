using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawners : MonoBehaviour
{
    [SerializeField]  GameObject[] spawners;
    [SerializeField]  GameObject[] enemies;
    [SerializeField] public int enemiesToSpawn;
    [SerializeField]  float spawnInterval;
    public int enemiesSpawned;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(SpawnEnemy());
        }
    }

    private IEnumerator SpawnEnemy()
    {
        while (enemiesSpawned < enemiesToSpawn)
        {
            int enemy = Random.Range(0, enemies.Length);
            int spawn = Random.Range(0, spawners.Length);
            Instantiate(enemies[enemy], spawners[spawn].transform.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
            enemiesSpawned++;
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
