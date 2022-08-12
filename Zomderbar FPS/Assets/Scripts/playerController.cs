using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("---------- Components -----------")]
    [SerializeField] CharacterController controller;
    [SerializeField] GameObject gunModel;

    [Header("---------- Player Attributes -----------")]
    [Range(1, 10)][SerializeField] float playerSpeed;
    [Range(1, 4)][SerializeField] float sprintMult;
    [Range(8, 18)][SerializeField] float jumpHeight;
    [Range(15, 30)][SerializeField] float gravityValue;
    [Range(1, 3)][SerializeField] int jumpMax;

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

    private int weapIndx;
    private int prevWeapIndx;

    private void Start()
    {
        playerSpeedOG = playerSpeed;
    }

    void Update()
    {
        playerMovement();
        sprint();

        //StartCoroutine(shoot());
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

                    isDamageable.takeDamage(shootDmg);
                }

                //if (hit.transform.CompareTag("Block"))
                //{
                //    Destroy(hit.transform.gameObject);
                //}
                //else
                //{
                //    Instantiate(cube, hit.point, cube.transform.rotation);
                //}
            }

            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
    }

    public void gunPickup(float _shootRate, float _shootDistance, int _shootDmg,GameObject _model, gunStats _gunStats)
    {
        shootRate = _shootRate;
        shootDistance = _shootDistance;    //Need to add gunStats
        shootDmg = _shootDmg;
        gunModel.GetComponent<MeshFilter>().sharedMesh = _model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = _model.GetComponent<MeshRenderer>().sharedMaterial;
        gunstat.Add(_gunStats);
    }

    //void gunSwitch()
    //{
    //    for (int i = 0; i < gunstat.Count; i++)
    //    {
    //        if (Input.GetKeyDown((i+1).ToString()))
    //        {

    //        }
    //    }
    //}

    //void EquipWeapon(int _index)
    //{
    //    if (_index == prevWeapIndx)
    //        return;

    //    weapIndx = _index;
    //    //gunstat[weapIndx]
    //}


}