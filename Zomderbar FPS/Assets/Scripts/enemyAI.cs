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
    //[SerializeField] Renderer rend;
    //[SerializeField] private Renderer rend;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject bulletSpawn;

    [Header("---------- Stats -----------")]
    [Range(0, 30)][SerializeField] int HP;
    [Range(1, 10)][SerializeField] float playerFaceSpeed;
    [Range(1, 30)] [SerializeField] int fieldOfViewShoot;
    [Range(1, 180)] [SerializeField] int fieldOfView;
    [Range(1, 5)][SerializeField] int speedRoam;
    [Range(1, 5)][SerializeField] int speedChase;
    [SerializeField] bool isZombieFloaty;
    
    private float distanceFromPlayer;

    [Header("---------- Weapon Stats -----------")]
    [Range(0.1f, 5)][SerializeField] float shootRate;
    [Range(1, 10)][SerializeField] int damage;
    [Range(0, 10)][SerializeField] int bulletSpeed;
    [Range(1, 5)][SerializeField] int bulletDstryTime;
    [Range(1, 10)] [SerializeField] private float shootRange;
    [Range(1, 15)] [SerializeField] private float visionRange;

     Vector3 playerDir;
    bool isShooting = false;

    // Start is called before the first frame update
    void Start()
    {
        agent.stoppingDistance = shootRange;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled && !anim.GetBool("Dead"))
        {
            anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agent.velocity.normalized.magnitude, Time.deltaTime * 5));
            playerDir = gameManager.instance.player.transform.position - head.position;

            distanceFromPlayer = Vector3.Distance(transform.position, gameManager.instance.player.transform.position);
            Debug.Log(distanceFromPlayer);

            if (agent.enabled == true)
            {
                CanSeePlayer();
            }
            if (distanceFromPlayer > visionRange && agent.remainingDistance < 0.1)
            {
                //Roam(); For now we don't want the zombies to roam arround, but rather to stay still until they "hear" something.
            }
        }

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
    private void CanSeePlayer()
    {
        float angle = Vector3.Angle(playerDir, head.forward);
        RaycastHit hit;
        if (Physics.Raycast(head.position, playerDir, out hit))
        {
            Debug.DrawRay(head.position, playerDir);
            if (hit.collider.CompareTag("Player") && !isShooting)
            {
                if (angle <= fieldOfView)
                {
                    if (distanceFromPlayer <= visionRange)
                    {
                        agent.stoppingDistance = shootRange;
                        agent.SetDestination(gameManager.instance.player.transform.position);
                        facePlayer(); 
                        if (distanceFromPlayer <= shootRange && angle <= fieldOfViewShoot)
                            StartCoroutine(shoot());
                    }
                    else
                    {
                        agent.stoppingDistance = 0.1f;
                    }
                }
            }
        }
    }


    public void takeDamage(int dmg)
    {
        anim.SetTrigger("Damage");
        HP -= dmg;

        if (HP > 0)
        {
            StartCoroutine(flashColor());
        }

        else
        {
            //Destroy(gameObject);
            anim.SetBool("Dead", true);
            if (isZombieFloaty)
                StartCoroutine(shiftToFloorTimer(0.25f));
            agent.enabled = false;
            
            foreach (Collider col in GetComponents<Collider>())
            {
                col.enabled = false;
            }
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

    IEnumerator flashColor()
    {
        //rend.material.color = Color.red;
        //agent.enabled = false;
        agent.speed = 0;
        yield return new WaitForSeconds(0.1f);
        //agent.enabled = true;
        agent.speed = speedChase;
        agent.stoppingDistance = 0;
        agent.SetDestination(gameManager.instance.player.transform.position);
        //rend.material.color = Color.white;
    }

    IEnumerator shiftToFloorTimer(float time)
    {
        yield return new WaitForSeconds(time);
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.8f, transform.position.z);
    }


}