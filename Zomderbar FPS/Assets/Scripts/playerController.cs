using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class playerController : MonoBehaviour, IDamageable
{
    [Header("---------- Components -----------")]
    public CharacterController controller;
    public AudioSource audioSource;
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject hitEffect;
    [SerializeField] GameObject bloodEffect;
    [SerializeField] Animator anim;

    [Header("---------- Player Attributes -----------")]
    [Range(1, 10)][SerializeField] public float playerSpeed;
    [Range(1.1f, 2)][SerializeField] float slideMult;
    [SerializeField] int slideTime;
    [SerializeField] float wallRunSpeed;
    [Range(8, 18)][SerializeField] float jumpHeight;
    [Range(15, 30)][SerializeField] public float gravityValue;
    [Range(1, 3)][SerializeField] public int jumpMax;
    [Range(0, 310)][SerializeField] public int hp;
    [Range(0.1f, 2)][SerializeField] float switchTime;
    [Range(0.1f, 2)][SerializeField] float doubleJumpHeightMult;
    [Range(0.01f, 0.1f)][SerializeField] float doubleJumpSpeedMult;

    [Header("---------- Gun Stats -----------")]
    [SerializeField] GameObject gunModel;
    [Range(0.1f, 5)][SerializeField] float shootRate;
    [Range(1, 30)][SerializeField] public float shootDistance;
    [Range(1, 10)][SerializeField] int shootDmg;
    public int[] currentAmmoCount = new int[6];
    int selectedWeapon;
    [SerializeField] float reloadTimer;

    [Header("--------- Audio ----------")]
    [SerializeField] AudioClip[] walking;
    [Range(0, 1)][SerializeField] float walkingVol;
    [SerializeField] AudioClip[] footfalls;
    [Range(0, 1)][SerializeField] float footfallsVol;
    public AudioClip[] audioJump;
    public AudioClip audioLand;
    [Range(0, 1)][SerializeField] float landingVol;
    public AudioClip[] audioDamaged;
    public AudioClip audioSlide;
    [SerializeField] AudioClip offWallJumpSound;
    [Range(0, 1)][SerializeField] float offWallJumpSoundVol;

    bool isShooting = false;
    bool alreadyReloadedUI = false;

    Vector3 playerVelocity;
    Vector3 move = Vector3.zero;

    public List<gunStats> gunstat = new List<gunStats>();
    int weapIndx;

    public int timesJumps;
    public int timesJumpsAudio;

    float playerSpeedOG;
    public int hpOriginal;
    int ammoCountOrig;
    float wallJumpSpeedOG;
    public float gravityValueOG;
    bool canSlide = true;
    bool isSliding = false;
    public bool isWallRun = true;
    public bool isSameWall = false;
    bool canShoot = true;
    bool isWalking = true;
    bool isJump = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        playerSpeedOG = playerSpeed;
        gravityValueOG = gravityValue;
        hpOriginal = hp;
        wallJumpSpeedOG = wallRunSpeed;
        ammoCountOrig = 0;
        audioSource = GetComponent<AudioSource>();
        controller.enabled = true;
        updatePlayerHp();
    }

    void Update()
    {
        if (transform.position.y < -30)
            respawn();

        if(isWalking && !isSliding)
            StartCoroutine(footsteps(0.4f));

        else if (isWalking && isSliding && playerSpeed <= playerSpeedOG/1.5f)
        {
            StartCoroutine(footsteps(0.9f));
        }

        playerMovement();
        slide();

        StartCoroutine(reload());

        StartCoroutine(gunSwitch());
        StartCoroutine(shoot());
    }

    /*
     * Player movement is at a constant running speed.
     */

    IEnumerator footsteps(float waitForSecs)
    {

        if (controller.isGrounded && move.normalized.magnitude > 0.3f)
        {
            isWalking = false;

            audioSource.PlayOneShot(walking[Random.Range(0, walking.Length)], walkingVol);

            yield return new WaitForSeconds(waitForSecs);

            isWalking = true;
        }
    }

    void playerMovement()
    {
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            timesJumps = 0;
            timesJumpsAudio = 0;
            isWallRun = false;
            //isJump = false;
            if (isJump)
            {
                audioSource.PlayOneShot(audioLand, landingVol);
                isJump = false;
            }
        }

        move = ((transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical")));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (Input.GetButtonDown("Jump") && timesJumps < jumpMax)
        {
            isJump = true;
            playerVelocity.y = jumpHeight;
            timesJumps++;
            audioSource.PlayOneShot(audioJump[(int)Random.Range(0, audioDamaged.Length - 1)]);
            if (timesJumps > 1)
            {
                if (isWallRun)
                {
                    playerVelocity.y = jumpHeight * doubleJumpHeightMult + 1.5f;
                    //playerSpeed *= wallRunSpeed;
                    audioSource.PlayOneShot(offWallJumpSound, offWallJumpSoundVol );
                    StartCoroutine(OffTheWall());

                }

                else
                    playerVelocity.y = jumpHeight * doubleJumpHeightMult;

            }
                
        }

        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    IEnumerator OffTheWall()
    {
        playerSpeed *= wallRunSpeed / 1.3f;
        yield return new WaitForSeconds(0.08f);
        playerSpeed = playerSpeedOG;
    }

    /*
     * Sliding allows the player to temporarily move faster.
     */
    void slide()
    {
        if (canSlide)
        {
            if (Input.GetButtonDown("Sprint"))
            {
                isSliding = true;
                StartCoroutine(slowSlide());
                //audioSource.PlayOneShot(audioSlide);
            }
            else if (Input.GetButtonUp("Sprint"))
            {
                isSliding = false;
                transform.localScale = new Vector3(1, 1, 1);
                playerSpeed = playerSpeedOG;
                StartCoroutine(standUp());
            }
        }
    }

    IEnumerator slowSlide()
    {
        controller.transform.localScale = new Vector3(1, 0.5f, 1);
        //audioSource.

        if (timesJumps > 0)
            playerSpeed = playerSpeed * slideMult + 0.5f;
        else
            playerSpeed = playerSpeed * slideMult;

        while (isSliding)
        {
            yield return new WaitForSeconds(0.2f);
            playerSpeed -= 0.3f;

            if (playerSpeed <= playerSpeedOG / 1.5)
                break;
        }
    }

    IEnumerator standUp()
    {
        canSlide = false;
        yield return new WaitForSeconds(0.5f);
        canSlide = true;
    }

    IEnumerator shoot()
    {
        if (canShoot)
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.red, 0.0000001f);
            if (gunstat.Count != 0 && Input.GetButton("Shoot") && currentAmmoCount[selectedWeapon] > 0 && isShooting == false && !gameManager.instance.isPaused)
            {
                isShooting = true;
                anim.SetTrigger("GunReoil");
                audioSource.PlayOneShot(gunstat[selectedWeapon].shootSound);

                gameManager.instance.currentGunHUD.transform.GetChild(0).GetChild(currentAmmoCount[selectedWeapon] - 1).gameObject.SetActive(false);
                currentAmmoCount[selectedWeapon]--;

                if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out RaycastHit hit, shootDistance))
                {
                    Instantiate(hitEffect, hit.point, hitEffect.transform.rotation);
                    if (hit.collider.GetComponent<IDamageable>() != null)
                    {
                        IDamageable isDamageable = hit.collider.GetComponent<IDamageable>();
                        
                        Instantiate(bloodEffect, hit.point, hit.transform.rotation);

                        /*
                         * Headshot handler
                         */
                        if (hit.collider is SphereCollider)
                            isDamageable.takeDamage(shootDmg * 2);
                        else
                            isDamageable.takeDamage(shootDmg);
                    }
                }
                StartCoroutine(CameraShake.Instance.ShakeCamera(0.15f, .15f));

                yield return new WaitForSeconds(shootRate);
                isShooting = false;
            }
        }
    }

    /*
     * Guns are added to a list for easy swapping between weapons
     */
    public void gunPickup(gunStats _gunStat, int _currentGunHUD)
    {
        for (int i = 0; i < gunstat.Count; i++)
        {
            if (gunstat[i].gunHUD == _currentGunHUD)
                return;
        }

        if (alreadyReloadedUI)
        {
            gameManager.instance.currentGunHUD.transform.GetChild(3).gameObject.SetActive(false);
            alreadyReloadedUI = false;
        }
        gunstat.Add(_gunStat);

        if (gunstat.Count != 0)
            selectedWeapon = gunstat.Count - 1;

        shootRate = _gunStat.shootRate;
        shootDistance = _gunStat.shootDist;
        shootDmg = _gunStat.shootDmg;
        currentAmmoCount[selectedWeapon] = _gunStat.ammoCapacity;
        ammoCountOrig = _gunStat.ammoCapacity;
        gunModel.GetComponent<MeshFilter>().sharedMesh = _gunStat.model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = _gunStat.model.GetComponent<MeshRenderer>().sharedMaterial;

        if (gameManager.instance.currentGunHUD != null)
            gameManager.instance.currentGunHUD.SetActive(false);

        gameManager.instance.currentGunHUD = gameManager.instance.gunHUD[_currentGunHUD];
        gameManager.instance.currentGunHUD.SetActive(true);

        for (int i = 0; i < currentAmmoCount[selectedWeapon]; ++i)
            gameManager.instance.currentGunHUD.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
    }

    /*
     * Switching guns takes time, which removes potential glitches involving fast gun switching.
     */
    IEnumerator gunSwitch()
    {
        if (gunstat.Count > 0 && !gameManager.instance.isPaused)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedWeapon < gunstat.Count - 1)
            {
                if (alreadyReloadedUI)
                {
                    gameManager.instance.currentGunHUD.transform.GetChild(3).gameObject.SetActive(false);
                    alreadyReloadedUI = false;
                }
                selectedWeapon++;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedWeapon > 0)
            {
                if (alreadyReloadedUI)
                {
                    gameManager.instance.currentGunHUD.transform.GetChild(3).gameObject.SetActive(false);
                    alreadyReloadedUI = false;
                }
                selectedWeapon--;
            }

            shootRate = gunstat[selectedWeapon].shootRate;
            shootDistance = gunstat[selectedWeapon].shootDist;
            shootDmg = gunstat[selectedWeapon].shootDmg;
            ammoCountOrig = gunstat[selectedWeapon].ammoCapacity;
            reloadTimer = gunstat[selectedWeapon].reloadTime;
            gunModel.GetComponent<MeshFilter>().sharedMesh = gunstat[selectedWeapon].model.GetComponent<MeshFilter>().sharedMesh;
            gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunstat[selectedWeapon].model.GetComponent<MeshRenderer>().sharedMaterial;

            gameManager.instance.currentGunHUD.SetActive(false);
            gameManager.instance.currentGunHUD = gameManager.instance.gunHUD[gunstat[selectedWeapon].gunHUD];
            gameManager.instance.currentGunHUD.SetActive(true);
            for (int i = 0; i < currentAmmoCount[selectedWeapon]; ++i)
                gameManager.instance.currentGunHUD.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);

            yield return new WaitForSeconds(switchTime);
        }
    }

    public void takeDamage(int dmg)
    {
        if (dmg > 0)
            StartCoroutine(damageFlash());

        hp -= dmg;
        audioSource.PlayOneShot(audioDamaged[(int)Random.Range(0, audioDamaged.Length - 1)]);
        updatePlayerHp();
        if (hp <= 0)
        {
            death();
            resetHP();
        }
    }

    /*
     * slow factor of 2, will half player speed.
     */
    public IEnumerator SlowPlayer(float slowFactor, float slowDuration)
    {
        playerSpeed = playerSpeed / slowFactor;
        yield return new WaitForSecondsRealtime(slowDuration);
        playerSpeed = playerSpeedOG;
    }

    public void respawn()
    {
        controller.enabled = false;
        transform.position = gameManager.instance.playerSpawnPoint.transform.position;
        controller.enabled = true;
        gameManager.instance.isPaused = false;
        updatePlayerHp();
        resetPlayerAmmo();
        StartCoroutine(resetEnemies());
    }

    IEnumerator resetEnemies()
    {
        enemyAI.resetPos = true;
        yield return new WaitForSeconds(0.1f);
        enemyAI.resetPos = false;
    }

    public void death()
    {
        gameManager.instance.cursorLockPause();
        gameManager.instance.currentMenuOpen = gameManager.instance.playerDeadMenu;
        gameManager.instance.currentMenuOpen.SetActive(true);

        /*
         * Menu Navigation
         * Clear selected object first.
         */
        EventSystem.current.SetSelectedGameObject(null);

        /*
         * Set new selected object
         */
        EventSystem.current.SetSelectedGameObject(gameManager.instance.deadFirstButton);
    }

    IEnumerator damageFlash()
    {
        gameManager.instance.playerDamageFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.playerDamageFlash.SetActive(false);
    }

    public void resetHP()
    {
        hp = hpOriginal;
    }

    /*
     * All guns are reloaded when a player respawns
     */
    public void resetPlayerAmmo()
    {
        for (int i = 0; i < gunstat.Count; ++i)
        {
            currentAmmoCount[i] = gunstat[i].ammoCapacity;
            for (int j = 0; j < gunstat[i].ammoCapacity; ++j)
                gameManager.instance.gunHUD[gunstat[i].gunHUD].transform.GetChild(0).GetChild(j).gameObject.SetActive(true);
        }
    }

    public void updatePlayerHp()
    {
        gameManager.instance.playerHpBar.fillAmount = (float)hp / (float)hpOriginal;
    }

    public IEnumerator reload()
    {
        if (Input.GetButtonDown("Reload") && gunstat.Count != 0)
        {
            if (currentAmmoCount[selectedWeapon] == ammoCountOrig && !alreadyReloadedUI)
            {
                StartCoroutine(alreadyReloaded());
            }
            else if (currentAmmoCount[selectedWeapon] != ammoCountOrig)
            {
                audioSource.PlayOneShot(gunstat[selectedWeapon].reloadSound);

                canShoot = false;
                anim.SetBool("Reloading", true);
                yield return new WaitForSeconds(reloadTimer - .25f);
                anim.SetBool("Reloading", false);
                yield return new WaitForSeconds(.25f);
                currentAmmoCount[selectedWeapon] = ammoCountOrig;
                for (int i = 0; i < currentAmmoCount[selectedWeapon]; ++i)
                    gameManager.instance.currentGunHUD.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
                canShoot = true;
            }
        }
    }

    /*
     * The player should know if they have a full magazine
     */
    IEnumerator alreadyReloaded()
    {
        alreadyReloadedUI = true;
        gameManager.instance.currentGunHUD.transform.GetChild(3).gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        gameManager.instance.currentGunHUD.transform.GetChild(3).gameObject.SetActive(false);
        alreadyReloadedUI = false;
    }

    IEnumerator slowJumpMovement()
    {
        playerSpeed = playerSpeed * doubleJumpSpeedMult;
        yield return new WaitForSeconds(1.8f);
        playerSpeed = playerSpeedOG;
    }
    void landAudio()
    {
       
    }
}