using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (gameManager.instance.playerScript.isSameWall == false && !gameManager.instance.playerScript.controller.isGrounded)
        {
            if (other.CompareTag("Player"))
            {
                gameManager.instance.playerScript.isSameWall = true;
                gameManager.instance.playerScript.gravityValue /= 2;
                gameManager.instance.playerScript.timesJumps = 1;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        gameManager.instance.playerScript.isSameWall = false;
        StartCoroutine(resetGravity());
    }

    IEnumerator resetGravity()
    {
        gameManager.instance.playerScript.gravityValue *=2;
        yield return new WaitForSeconds(0.03f);
        gameManager.instance.playerScript.gravityValue = gameManager.instance.playerScript.gravityValueOG;
    }
}
