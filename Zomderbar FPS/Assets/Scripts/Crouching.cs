using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouching : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] float crouchSpeed;

    bool isCrouch = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        crouch();
    }

    void crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && isCrouch == false)
        {
            isCrouch = true;
            gameManager.instance.player.transform.localScale = new Vector3(1, .5f, 1);
            gameManager.instance.playerScript.playerSpeed = crouchSpeed;
            
        }
    }
}
