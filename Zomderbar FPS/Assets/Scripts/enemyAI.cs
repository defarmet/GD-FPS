using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamageable
{
    [Header("---------- Components -----------")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform    head;
    [SerializeField] Animator     anim;
    [SerializeField] GameObject   bullet;
    [SerializeField] GameObject   bulletSpawn;
    AudioSource source;

    [Header("---------- Audio -----------")]
    [SerializeField]               bool        isNoisy;
    [Range(0, 1)] [SerializeField] float       volume;
    [SerializeField]               AudioClip[] atackAudioClips;
    [SerializeField]               AudioClip[] damagedAudioClips;

    [Header("---------- Stats -----------")]
    [Range(0, 300)] public           int   HP;
    [Range(1, 180)] [SerializeField] int   fieldOfView;
    [Range(1, 90)]  [SerializeField] int   fieldOfViewShoot;
    [Range(1, 10)]  [SerializeField] float playerFaceSpeed;
    [Range(1, 15)]  [SerializeField] int   speedRoam;
    [Range(1, 15)]  [SerializeField] int   speedChase;
    [SerializeField]                 float animationBuffer;

    [Header("---------- Weapon Stats -----------")]
    [Range(0.1f, 5)] [SerializeField] float         shootRate;
    [Range(1, 10)]   [SerializeField] int           damage;
    [Range(0, 20)]   [SerializeField] int           bulletSpeed;
    [Range(1, 5)]    [SerializeField] int           bulletDestroyTime;
    [Range(1, 50)]   [SerializeField] private float shootRange;
    [Range(1, 50)]   [SerializeField] private float visionRange;

    Vector3 playerDir;
    float   distanceFromPlayer;
    bool    isShooting = false;

                  Vector3 startPos;
    public static bool    resetPos = false;
    
    void Start()
    {
        startPos = transform.position;
        agent.stoppingDistance = shootRange * 0.8f;
        anim.SetBool("Dead", false);
        if(isNoisy) {
            source = GetComponent<AudioSource>();
            source.volume = volume;
        }
    }

    /*
     * Lets the enemy know where the player is at all times.
     * However, the enemy doesn't always act on that knowledge.
     */
    void Update()
    {
        if (agent.isActiveAndEnabled) {
            if (resetPos)
                transform.position = startPos;

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
                    agent.stoppingDistance = shootRange * 0.8f;
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
        if (agent.remainingDistance <= agent.stoppingDistance && isShooting == false) {
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

            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
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
        SoundAtack();
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
        SoundTakeDamage();
        yield return new WaitForSeconds(0.1f);

        agent.speed = speedChase;
        agent.stoppingDistance = 0;
        agent.SetDestination(gameManager.instance.player.transform.position);
    }

    private void SoundTakeDamage()
    {
        if(isNoisy && damagedAudioClips.Length > 0) {
            source.clip = damagedAudioClips[Random.Range(0, damagedAudioClips.Length)];
            source.PlayOneShot(source.clip, volume);
        }
    }

    private void SoundAtack()
    {
        if (isNoisy && atackAudioClips.Length > 0) {
            source.clip = atackAudioClips[Random.Range(0, atackAudioClips.Length)];
            source.PlayOneShot(source.clip, volume);
        }
    }

    public void SetBullet(GameObject newBullet)
    {
        bullet = newBullet;
    }
    public void SetSpeed(int newSpeed)
    {
        speedChase = newSpeed;
        speedRoam = newSpeed;
        agent.speed = speedChase;
    }

    public void SetShootRate(float newRate)
    {
        shootRate = newRate;
    }

    public void SetShootRange(float newRange)
    {
        shootRange = newRange;
        agent.stoppingDistance = shootRange * 0.8f;
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    public void SetBulletSpeed(int newBulletSpeed)
    {
        bulletSpeed = newBulletSpeed;
    }

    public void SetBulletTime(int bulletTime)
    {
        bulletDestroyTime = bulletTime;
    }

    public void SetAnimBuffer(float newBuffer)
    {
        animationBuffer = newBuffer;
    }
}