using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, IDamageable
{
    [Header("---------- Components -----------")]
    [SerializeField] CharacterController controller;

    [Header("---------- Player Attributes -----------")]
    [Range(1, 10)] [SerializeField] float playerSpeed;
    [Range(1, 10)] [SerializeField] float crouchSpeed;
    [Range(1.1f, 2)] [SerializeField] float slideMulti;
    [Range(1, 4)] [SerializeField] float sprintMult;
    [Range(8, 18)] [SerializeField] float jumpHeight;
    [Range(15, 30)] [SerializeField] float gravityValue;
    [Range(1, 3)] [SerializeField] int jumpMax;
    [Range(0, 310)] [SerializeField] public int hp;

    [Header("---------- Gun Stats -----------")]
    [Range(0.1f, 5)] [SerializeField] float shootRate;
    [Range(1, 30)] [SerializeField] float shootDistance;
    [Range(1, 10)] [SerializeField] int shootDmg;
    [Range(0, 20)] [SerializeField] int ammoCount;
    [SerializeField] int selectedWeapon;
    [SerializeField] List<gunStats> gunstat = new List<gunStats>();


    //[SerializeField] GameObject cube;

    private Vector3 playerVelocity;
    Vector3 move = Vector3.zero;
    int timesJumps;
    float playerSpeedOG;
    bool isSprinting = false;
    bool isShooting = false;
    int hpOriginal;
    int ammoCountOrig;
    bool isCrouching = false;
    float gravityValueOG;



    private int weapIndx;
    //private int prevWeapIndx;

    private void Start()
    {
        playerSpeedOG = playerSpeed;
        gravityValueOG = gravityValue;
        hpOriginal = hp;
        ammoCountOrig = ammoCount;
        updatePlayerHp();

        controller.enabled = true;
    }

    void Update()
    {
        playerMovement();
        sprint();
        reload();
        gunSwitch();
        Crouching();

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
        }

        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            isSprinting = true;
            playerSpeed = playerSpeed * sprintMult;
        }

        if (Input.GetButtonUp("Sprint"))
        {
            isSprinting = false;
            playerSpeed = playerSpeedOG;
        }
    }

    IEnumerator shoot()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.red, 0.0000001f);
        //Debug.DrawLine(Camera.main.transform.position)

        if (gunstat.Count != 0 && Input.GetButton("Shoot") && ammoCount > 0 && isShooting == false)
        {
            isShooting = true;
            gameManager.instance.currentGunHUD.transform.GetChild(0).GetChild(ammoCount - 1).gameObject.SetActive(false);
            --ammoCount;

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

    public void gunPickup(float _shootRate, float _shootDistance, int _shootDmg, int _ammoCount, GameObject _currentGunHUD, gunStats _gunStats)
    {
        for (int i = 0; i < ammoCountOrig; ++i)
            gameManager.instance.currentGunHUD.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);

        if (gunstat.Count != 0)
        {
            selectedWeapon = gunstat.Count - 1;
            ++selectedWeapon;
        }
        shootRate = _shootRate;
        shootDistance = _shootDistance;
        shootDmg = _shootDmg;
        ammoCount = _ammoCount;
        ammoCountOrig = ammoCount;

        if (gameManager.instance.currentGunHUD != null)
            gameManager.instance.currentGunHUD.SetActive(false);
        gameManager.instance.currentGunHUD = _currentGunHUD;
        gameManager.instance.currentGunHUD.SetActive(true);

        _gunStats.shootDmg = shootDmg;
        _gunStats.shootRate = shootRate;
        _gunStats.shootDist = shootDistance;
        _gunStats.gunHUD = gameManager.instance.currentGunHUD;
        gunstat.Add(_gunStats);

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

        if (gunstat.Count > 0)
        {

            if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedWeapon < gunstat.Count - 1)
            {
                for (int i = 0; i < ammoCountOrig; ++i)
                    gameManager.instance.currentGunHUD.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
                selectedWeapon++;
                shootRate = gunstat[selectedWeapon].shootRate;
                shootDistance = gunstat[selectedWeapon].shootDist;
                shootDmg = gunstat[selectedWeapon].shootDmg;
                ammoCountOrig = gunstat[selectedWeapon].ammoCapacity;
                ammoCount = ammoCountOrig;
                gameManager.instance.currentGunHUD.SetActive(false);
                gameManager.instance.currentGunHUD = gunstat[selectedWeapon].gunHUD;
                gameManager.instance.currentGunHUD.SetActive(true);

            }

            else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedWeapon > 0)
            {
                for (int i = 0; i < ammoCountOrig; ++i)
                    gameManager.instance.currentGunHUD.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
                selectedWeapon--;
                shootRate = gunstat[selectedWeapon].shootRate;
                shootDistance = gunstat[selectedWeapon].shootDist;
                shootDmg = gunstat[selectedWeapon].shootDmg;
                ammoCountOrig = gunstat[selectedWeapon].ammoCapacity;
                ammoCount = ammoCountOrig;
                gameManager.instance.currentGunHUD.SetActive(false);
                gameManager.instance.currentGunHUD = gunstat[selectedWeapon].gunHUD;
                gameManager.instance.currentGunHUD.SetActive(true);

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
        ammoCount = ammoCountOrig;
        for (int i = 0; i < gunstat.Count; ++i)
        {
            for (int j = 0; j < gunstat[i].ammoCapacity; ++j)
            {
                gunstat[i].gunHUD.transform.GetChild(0).GetChild(j).gameObject.SetActive(true);

            }

        }
    }


    public void updatePlayerHp()
    {
        gameManager.instance.playerHpBar.fillAmount = (float)hp / (float)hpOriginal;
    }



    public void reload()
    {
        if (Input.GetButtonDown("Reload"))
        {
            ammoCount = ammoCountOrig;
            for (int i = 0; i < ammoCountOrig; ++i)
                gameManager.instance.currentGunHUD.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);

        }
    }

    void Crouching()
    {
        //int i = 0;

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (isSprinting && controller.isGrounded)
            {
                isCrouching = true;
                transform.localScale = new Vector3(1, .5f, 1);
                playerSpeed *= slideMulti;

                StartCoroutine(StopSlide());
            }
            else
            {
                isCrouching = true;
                transform.localScale = new Vector3(1, .5f, 1);
                playerSpeed = crouchSpeed;
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isCrouching = false;
            transform.localScale = new Vector3(1, 1, 1);
            playerSpeed = playerSpeedOG;
        }
    }

    IEnumerator StopSlide()
    {
        //slideMulti -= 0.2f;
        isSprinting = true;
        yield return new WaitForSeconds(3);
        isSprinting = false;
        playerSpeed = crouchSpeed;
    }



}