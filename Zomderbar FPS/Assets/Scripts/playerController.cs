using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, IDamageable
{
    [Header("---------- Components -----------")]
    [SerializeField] CharacterController controller;
    [SerializeField] Rigidbody rb;

    [Header("---------- Player Attributes -----------")]
    [Range(1, 10)] [SerializeField] public float playerSpeed;
    //[Range(1, 10)] [SerializeField] public float crouchSpeed;
    [Range(1.1f, 2)] [SerializeField] float slideMulti;
    [SerializeField] int slideTime;
    [SerializeField] float wallRunSpeed;
    [Range(1, 4)] [SerializeField] float sprintMult;
    [Range(8, 18)] [SerializeField] float jumpHeight;
    [Range(15, 30)] [SerializeField] float gravityValue;
    [Range(1, 3)] [SerializeField] int jumpMax;
    [Range(0, 310)] [SerializeField] public int hp;

    [Header("---------- Gun Stats -----------")]
    [Range(0.1f, 5)] [SerializeField] float shootRate;
    [Range(1, 30)] [SerializeField] float shootDistance;
    [Range(1, 10)] [SerializeField] int shootDmg;
    [SerializeField] int selectedWeapon;
    [SerializeField] List<gunStats> gunstat = new List<gunStats>();


    //[SerializeField] GameObject cube;

    private Vector3 playerVelocity;
    Vector3 move = Vector3.zero;
    int timesJumps;
    public float playerSpeedOG;
    public bool isSprinting = false;
    public bool isShooting = false;
    int hpOriginal;
    int ammoCountOrig;
    //bool isCrouching = false;
    float gravityValueOG;
    public bool canSprint = true;
    public bool isWallrun = false;
    public bool alreadyReloadedUI = false;

    public int[] currentAmmoCount = new int[6];




    private int weapIndx;
    //private int prevWeapIndx;

    private void Start()
    {
        playerSpeedOG = playerSpeed;
        gravityValueOG = gravityValue;
        hpOriginal = hp;
        ammoCountOrig = 0;
        updatePlayerHp();

        controller.enabled = true;
    }

    void Update()
    {

        if (playerVelocity.y < -30)
            respawn();

        playerMovement();

        reload();
        gunSwitch();

        slide();

        StartCoroutine(shoot());
    }

    void playerMovement()
    {
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            timesJumps = 0;
        }

        //get input from Unity's input system
        move = ((transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"))); // fps movement

        // add move vector to character controller
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && timesJumps < jumpMax)
        {
            playerVelocity.y = jumpHeight;
            timesJumps++;

            if (timesJumps > 1)
            {
                playerVelocity.y = jumpHeight * 1.6f;
                StartCoroutine(slowJumpMovement());

            }
        }

        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    // public void sprint()
    //{
    //        if (Input.GetButtonDown("Sprint"))
    //        {
    //            isSprinting = true;
    //            playerSpeed = playerSpeed * sprintMult;
    //        }

    //        if (Input.GetButtonUp("Sprint"))
    //        {
    //            isSprinting = false;
    //            playerSpeed = playerSpeedOG;
    //        }
    //}

    void slide()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            transform.localScale = new Vector3(1, .5f, 1);
            playerSpeed = playerSpeed * slideMulti;
            StartCoroutine(stopSlide());

        }

        else if (Input.GetButtonUp("Sprint"))
        {
            transform.localScale = new Vector3(1, 1, 1);
            playerSpeed = playerSpeedOG;
        }
    }

    IEnumerator stopSlide()
    {
        yield return new WaitForSeconds(slideTime);
        //transform.localScale = new Vector3(1, 1, 1);
        playerSpeed = playerSpeedOG;

    }

    IEnumerator shoot()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.red, 0.0000001f);
        //Debug.DrawLine(Camera.main.transform.position)

        if (gunstat.Count != 0 && Input.GetButton("Shoot") && currentAmmoCount[selectedWeapon] > 0 && isShooting == false && !gameManager.instance.isPaused)
        {
            isShooting = true;
            gameManager.instance.currentGunHUD.transform.GetChild(0).GetChild(currentAmmoCount[selectedWeapon] - 1).gameObject.SetActive(false);
            currentAmmoCount[selectedWeapon]--;

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDistance)) //if it hits something
            {

                if (hit.collider.GetComponent<IDamageable>() != null)
                {
                    IDamageable isDamageable = hit.collider.GetComponent<IDamageable>();
                    //headshot
                    if (hit.collider is SphereCollider)
                        isDamageable.takeDamage(shootDmg * 2);

                    else
                        isDamageable.takeDamage(shootDmg);
                }
            }

            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
    }

    public void gunPickup(gunStats _gunStat, int _currentGunHUD)
    {
        if (alreadyReloadedUI)
        {
            gameManager.instance.currentGunHUD.transform.GetChild(3).gameObject.SetActive(false);
            alreadyReloadedUI = false;
        }
        gunstat.Add(_gunStat);

        if (gunstat.Count != 0)
        {
            selectedWeapon = gunstat.Count - 1;
        }
        shootRate = _gunStat.shootRate;
        shootDistance = _gunStat.shootDist;
        shootDmg = _gunStat.shootDmg;
        currentAmmoCount[selectedWeapon] = _gunStat.ammoCapacity;
        ammoCountOrig = _gunStat.ammoCapacity;

        if (gameManager.instance.currentGunHUD != null)
            gameManager.instance.currentGunHUD.SetActive(false);
        gameManager.instance.currentGunHUD = gameManager.instance.gunHUD[_currentGunHUD];
        gameManager.instance.currentGunHUD.SetActive(true);

        for (int i = 0; i < currentAmmoCount[selectedWeapon]; ++i)
            gameManager.instance.currentGunHUD.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);


    }

    void gunSwitch()
    {
        //if (gunstat.Count > 0) //If you have at least 1 weapon
        //{
        //    if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
        //    {
        //        if (weapIndx >= gunstat.Count - 1) //You have your weapon on the last spot of your inventory equiped
        //        {
        //            weapIndx = 0; //Go back to the first (Toroid wrap)
        //        }
        //        else
        //        {
        //            weapIndx += 1; //Go to your next weapon
        //        }
        //    }
        //    else if (Input.GetAxis("Mouse ScrollWheel") < 0) // backwards
        //    {
        //        if (weapIndx <= 0) //You have your weapon on the first spot of your inventory equiped
        //        {
        //            weapIndx = gunstat.Count - 1; //Go back to the last (Toroid wrap)
        //        }
        //        else
        //        {
        //            weapIndx -= 1; //Go to your previous weapon
        //        }
        //    }

        //    shootRate = gunstat[weapIndx].shootRate;
        //    shootDmg = gunstat[weapIndx].shootDmg;
        //    shootDistance = gunstat[weapIndx].shootDist;
        //}

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
                shootRate = gunstat[selectedWeapon].shootRate;
                shootDistance = gunstat[selectedWeapon].shootDist;
                shootDmg = gunstat[selectedWeapon].shootDmg;
                ammoCountOrig = gunstat[selectedWeapon].ammoCapacity;
                gameManager.instance.currentGunHUD.SetActive(false);
                gameManager.instance.currentGunHUD = gameManager.instance.gunHUD[gunstat[selectedWeapon].gunHUD];
                gameManager.instance.currentGunHUD.SetActive(true);
                for (int i = 0; i < currentAmmoCount[selectedWeapon]; ++i)
                    gameManager.instance.currentGunHUD.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);

            }

            else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedWeapon > 0)
            {
                if (alreadyReloadedUI)
                {
                    gameManager.instance.currentGunHUD.transform.GetChild(3).gameObject.SetActive(false);
                    alreadyReloadedUI = false;
                }
                selectedWeapon--;
                shootRate = gunstat[selectedWeapon].shootRate;
                shootDistance = gunstat[selectedWeapon].shootDist;
                shootDmg = gunstat[selectedWeapon].shootDmg;
                ammoCountOrig = gunstat[selectedWeapon].ammoCapacity;
                gameManager.instance.currentGunHUD.SetActive(false);
                gameManager.instance.currentGunHUD = gameManager.instance.gunHUD[gunstat[selectedWeapon].gunHUD];
                gameManager.instance.currentGunHUD.SetActive(true);

                for (int i = 0; i < currentAmmoCount[selectedWeapon]; ++i)
                    gameManager.instance.currentGunHUD.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);

            }
        }


    }

    public void takeDamage(int dmg)
    {
        hp -= dmg;
        StartCoroutine(damageFlash());
        updatePlayerHp();

        if (hp <= 0)
        {
            //kill player
            death();
            resetHP();

        }
    }

    public void respawn()
    {
        controller.enabled = false;
        transform.position = gameManager.instance.playerSpawnPoint.transform.position;
        controller.enabled = true;
        gameManager.instance.isPaused = false;
        updatePlayerHp();
        resetPlayerAmmo();
    }

    public void death()
    {
        gameManager.instance.cursorLockPause();
        gameManager.instance.currentMenuOpen = gameManager.instance.playerDeadMenu;
        gameManager.instance.currentMenuOpen.SetActive(true);
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
    public void resetPlayerAmmo()
    {
        selectedWeapon = 0;
        for (int i = 0; i < gunstat.Count; ++i)
        {
            currentAmmoCount[selectedWeapon] = gunstat[i].ammoCapacity;
            if (selectedWeapon < gunstat.Count - 1)
                ++selectedWeapon;
            for (int j = 0; j < gunstat[i].ammoCapacity; ++j)
            {
                gameManager.instance.gunHUD[gunstat[i].gunHUD].transform.GetChild(0).GetChild(j).gameObject.SetActive(true);

            }

        }
    }


    public void updatePlayerHp()
    {
        gameManager.instance.playerHpBar.fillAmount = (float)hp / (float)hpOriginal;
    }



    public void reload()
    {
        if (Input.GetButtonDown("Reload") && gunstat.Count != 0)
        {
            if (currentAmmoCount[selectedWeapon] == ammoCountOrig && !alreadyReloadedUI)
                StartCoroutine(alreadyReloaded());
            else if (currentAmmoCount[selectedWeapon] != ammoCountOrig)
            {
                currentAmmoCount[selectedWeapon] = ammoCountOrig;
                for (int i = 0; i < currentAmmoCount[selectedWeapon]; ++i)
                    gameManager.instance.currentGunHUD.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
            }

        }
    }

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
        playerSpeed = playerSpeed * 0.5f;
        yield return new WaitForSeconds(1.8f);
        playerSpeed = playerSpeedOG;
    }

    //void Crouching()
    //{
    //    //int i = 0;

    //    if (Input.GetKeyDown(KeyCode.LeftControl))
    //    {

    //        isCrouching = true;
    //        transform.localScale = new Vector3(1, .5f, 1);
    //        playerSpeed = crouchSpeed;

    //    }
    //    else if (Input.GetKeyDown(KeyCode.LeftControl) && isSprinting && isCrouching == false)
    //    {
    //        isCrouching = true;
    //        transform.localScale = new Vector3(1, .5f, 1);
    //        playerSpeed *= slideMulti;

    //        StartCoroutine(StopSlide());
    //    }
    //    else if (Input.GetKeyUp(KeyCode.LeftControl))
    //    {
    //        isCrouching = false;
    //        transform.localScale = new Vector3(1, 1, 1);
    //        playerSpeed = playerSpeedOG;
    //    }
    //}

}