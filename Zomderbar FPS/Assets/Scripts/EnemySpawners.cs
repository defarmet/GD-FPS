using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawners : MonoBehaviour
{
    [SerializeField] GameObject[] spawners;
    [SerializeField] GameObject[] enemies;
    public           int          enemiesToSpawn;
    [SerializeField] float        spawnInterval;
    public           int          enemiesSpawned;
    public           bool         spawnerActivated = false;
    
    GameObject boss;
    enemyAI    bossScript;
    bool       bossSpawned;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !spawnerActivated) {
            spawnerActivated = true;
            StartCoroutine(SpawnEnemy());
        }
    }

    private IEnumerator SpawnEnemy()
    {
        while (enemiesSpawned < enemiesToSpawn) {
            int enemy = Random.Range(0, enemies.Length);
            int spawn = Random.Range(0, spawners.Length);
            Instantiate(enemies[enemy], spawners[spawn].transform.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
            enemiesSpawned++;
        }
    }
}
