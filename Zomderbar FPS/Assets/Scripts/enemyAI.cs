using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamageable
{
    [Header("---------- Components -----------")]
    [SerializeField] NavMeshAgent agent;

    [Header("---------- Stats -----------")]
    [Range(0, 10)][SerializeField] int HP;
    [Range(1, 10)][SerializeField] float playerFaceSpeed;

    [Header("---------- Weapon Stats -----------")]
    [Range(0.1f, 5)][SerializeField] float shootRate;
    [Range(1, 10)][SerializeField] int damage;
    [Range(1, 10)][SerializeField] int bulletSpeed;
    [Range(1, 5)][SerializeField] int bulletDstryTime;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject bulletSpawn;

    Vector3 playerDir;
    bool isShooting = false;
    bool playerInRange = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(gameManager.instance.player.transform.position);
        playerDir = gameManager.instance.player.transform.position - transform.position;

        facePlayer();

        if (!isShooting && playerInRange)
            StartCoroutine(shoot());

    }

    void facePlayer()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            playerDir.y = 0;
            Quaternion rotation = Quaternion.LookRotation(playerDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * playerFaceSpeed);
        }
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;

        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        GameObject bulletClone = Instantiate(bullet, bulletSpawn.transform.position, bullet.transform.rotation);
        bulletClone.GetComponent<bullet>().damage = damage;
        bulletClone.GetComponent<bullet>().speed = bulletSpeed;
        bulletClone.GetComponent<bullet>().destroyTime = bulletDstryTime;

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

}