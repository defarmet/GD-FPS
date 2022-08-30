using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamageable
{
    [Header("---------- Components -----------")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] private Transform head;
    [SerializeField] private Animator anim;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject bulletSpawn;

    [Header("---------- Stats -----------")]
    [Range(0, 150)] [SerializeField] int   HP;
    [Range(1, 180)] [SerializeField] int   fieldOfView;
    [Range(1, 90)]  [SerializeField] int   fieldOfViewShoot;
    [Range(1, 10)]  [SerializeField] float playerFaceSpeed;
    [Range(1, 5)]   [SerializeField] int   speedRoam;
    [Range(1, 5)]   [SerializeField] int   speedChase;
    [SerializeField]                 float animationBuffer;

    [Header("---------- Weapon Stats -----------")]
    [Range(0.1f, 5)] [SerializeField] float         shootRate;
    [Range(1, 10)]   [SerializeField] int           damage;
    [Range(0, 20)]   [SerializeField] int           bulletSpeed;
    [Range(1, 5)]    [SerializeField] int           bulletDestroyTime;
    [Range(1, 50)]   [SerializeField] private float shootRange;
    [Range(1, 50)]   [SerializeField] private float visionRange;

    Vector3       playerDir;
    private float distanceFromPlayer;
    bool          isShooting = false;

    void Start()
    {
        agent.stoppingDistance = shootRange;
        anim.SetBool("Dead", false);
    }

    /*
     * Lets the enemy know where the player is at all times.
     * However, the enemy doesn't always act on that knowledge.
     */
    void Update()
    {
        if (agent.isActiveAndEnabled) {
            anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agent.velocity.normalized.magnitude, Time.deltaTime * 5));
            playerDir = gameManager.instance.player.transform.position - head.position;
            distanceFromPlayer = Vector3.Distance(transform.position, gameManager.instance.player.transform.position);

            if (agent.enabled)
                CanSeePlayer();
        }
    }

    /*
     * The enemy acts on the knowledge of where the player is and attacks when within range.
     */
    private void CanSeePlayer()
    {
        float angle = Vector3.Angle(new Vector3(playerDir.x, head.forward.y, playerDir.z), head.forward);
        if (Physics.Raycast(head.position, playerDir, out RaycastHit hit)) {
            Debug.DrawRay(head.position, playerDir);
            agent.SetDestination(gameManager.instance.player.transform.position);
            if (hit.collider.CompareTag("Player") && !isShooting && angle <= fieldOfView) {
                if (distanceFromPlayer <= visionRange) {
                    agent.stoppingDistance = shootRange;
                    facePlayer(); 
                    
                    if (distanceFromPlayer <= shootRange && angle <= fieldOfViewShoot)
                        StartCoroutine(shoot());
                } else {
                    agent.stoppingDistance = 0.1f;
                }
            }
        }
    }

    void facePlayer()
    {
        if (agent.remainingDistance <= agent.stoppingDistance) {
            playerDir.y = 0;
            Quaternion rotation = Quaternion.LookRotation(playerDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * playerFaceSpeed);
        }
    }

    public void takeDamage(int dmg)
    {
        if (dmg > 0)
            anim.SetTrigger("Damage");

        HP -= dmg;
        if (HP > 0) {
            StartCoroutine(flashColor());
        } else {
            gameManager.instance.enemyKilled++;
            anim.SetBool("Dead", true);
            agent.enabled = false;
            
            foreach (Collider col in GetComponents<Collider>())
                col.enabled = false;
        }
    }

    /*
     * The enemy needs to shoot/attack when the animation reaches the correct point.
     */
    IEnumerator shoot()
    {
        isShooting = true;
        anim.SetTrigger("Shoot");
        yield return new WaitForSeconds(animationBuffer);

        GameObject bulletClone = Instantiate(bullet, bulletSpawn.transform.position, bullet.transform.rotation);
        bulletClone.GetComponent<bullet>().damage = damage;
        bulletClone.GetComponent<bullet>().speed = bulletSpeed;
        bulletClone.GetComponent<bullet>().destroyTime = bulletDestroyTime;
        yield return new WaitForSeconds(shootRate);

        isShooting = false;
    }

    IEnumerator flashColor()
    {
        agent.speed = 0;
        anim.SetTrigger("Damage");
        yield return new WaitForSeconds(0.1f);

        agent.speed = speedChase;
        agent.stoppingDistance = 0;
        agent.SetDestination(gameManager.instance.player.transform.position);
    }
}