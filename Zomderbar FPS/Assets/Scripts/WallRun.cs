using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{

    [Header("Wall Running")]

    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float maxWallRunTimer;
    public float wallRunSpeed;
    float wallRunTimer;


    [Header("Wall Running")]

    private float horizontalInput;
    private float verticalInput;

    [Header("Wall Running")]

    public float wallCheckDistance;
    public float minJumpHeight;
    RaycastHit leftWallHit;
    RaycastHit rightWallHit;
    bool wallLeft;
    bool wallRight;

    [Header("References")]
    public Transform orientation;
    [SerializeField] playerController controller;
    [SerializeField] Rigidbody body;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkForWall();
    }

    void checkForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);
    }

    bool aboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    void StateMachine()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if((wallLeft || wallRight) && verticalInput > 0 && aboveGround())
        {

        }

    }

    void StartWallRun()
    {
        gameManager.instance.playerScript.isWallrun = true;
        gameManager.instance.playerScript.playerSpeed = wallRunSpeed;
    }

    void WallRunningMovement()
    {
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);
    }

    void StopWallRun()
    {

    }
}
