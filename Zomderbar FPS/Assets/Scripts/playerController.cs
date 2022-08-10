using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("---------- Components -----------")]
    [SerializeField] CharacterController controller;

    [Header("---------- Player Attributes -----------")]
    [Range(1, 10)][SerializeField] float playerSpeed;
    [Range(1, 4)][SerializeField] float sprintMult;
    [Range(8, 18)][SerializeField] float jumpHeight;
    [Range(15, 30)][SerializeField] float gravityValue;
    [Range(1, 3)][SerializeField] int jumpMax;
    [Range(0, 30)][SerializeField] public int hp;

    [Header("---------- Gun Stats -----------")]
    [Range(0.1f, 5)][SerializeField] float shootRate;
    [Range(1, 30)][SerializeField] float shootDistance;
    [Range(1, 10)][SerializeField] int shootDmg;
    [SerializeField] List<gunStats> gunstat = new List<gunStats>();

    //[SerializeField] GameObject cube;

    private Vector3 playerVelocity;
    Vector3 move = Vector3.zero;
    int timesJumps;
    float playerSpeedOG;
    bool isSprinting = false;
    bool isShooting = false;
    int ogHP;

    private int weapIndx;
    //private int prevWeapIndx;

    private void Start()
    {
        playerSpeedOG = playerSpeed;
        ogHP = hp;
    }

    void Update()
    {
        //DEBUG CODE
        if (Input.GetKeyDown(KeyCode.L))
            takeDamage(1);
        //


        playerMovement();
        sprint();
        gunSwitch();

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

    IEnumerator shoot() //Need to implement gunstat and IDamageable
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.red, 0.0000001f);
        //Debug.DrawLine(Camera.main.transform.position)

        if (gunstat.Count != 0 && Input.GetButton("Shoot") && isShooting == false)
        {
            isShooting = true;

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDistance)) //if it hits something
            {

                if (hit.collider.GetComponent<IDamageable>() != null)
                {
                    IDamageable isDamageable = hit.collider.GetComponent<IDamageable>();

                    if (hit.collider is SphereCollider)
                        isDamageable.takeDamage(shootDmg * 3);
                    else
                        isDamageable.takeDamage(shootDmg);
                }
            }

            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
    }

    public void gunPickup(float _shootRate, int _shootDistance, int _shootDmg, gunStats _gunStats)
    {
        shootRate = _shootRate;
        shootDistance = _shootDistance;    //Need to add gunStats
        shootDmg = _shootDmg;

        gunstat.Add(_gunStats);
    }

    void gunSwitch()
    {
        if (gunstat.Count > 0)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                if (weapIndx >= gunstat.Count - 1)
                {
                    weapIndx = 0;
                }
                else
                {
                    weapIndx += 1;
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                if (weapIndx <= 0)
                {
                    weapIndx = gunstat.Count - 1;
                }
                else
                {
                    weapIndx -= 1;
                }
            }

            shootRate = gunstat[weapIndx].shootRate;
            shootDistance = gunstat[weapIndx].shootDist;
            shootDmg = gunstat[weapIndx].shootDmg;
        }
    }

    public void takeDamage(int dmg)
    {
        hp -= dmg;
        StartCoroutine(damageFlash());

        if (hp <= 0)
        {
            death();
        }
    }

    public void respawn()
    {
        controller.enabled = false;
        transform.position = gameManager.instance.playerSpawnPoint.transform.position;
        controller.enabled = true;
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
        hp = ogHP;
    }
}