using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{

    [SerializeField]               AudioSource playerAudio;
    [SerializeField]               AudioClip   notif;
    [Range(0, 1)] [SerializeField] float       notifVol;

    private void OnTriggerEnter(Collider other)
    {
        if (!gameManager.instance.playerScript.isSameWall && !gameManager.instance.playerScript.controller.isGrounded) {
            if (other.CompareTag("Player")) {
                playerAudio.PlayOneShot(notif, notifVol);
                gameManager.instance.playerScript.isSameWall = true;
                gameManager.instance.playerScript.isWallRun = true;
                StartCoroutine(wallRunGrav());
                gameManager.instance.playerScript.timesJumps = 1;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(resetGravity());
    }

    IEnumerator wallRunGrav()
    {
        gameManager.instance.playerScript.gravityValue *= 2.6f;
        yield return new WaitForSeconds(0.02f);
        gameManager.instance.playerScript.gravityValue = gameManager.instance.playerScript.gravityValueOG;
        gameManager.instance.playerScript.gravityValue /= 2f;
    }

    IEnumerator resetGravity()
    {
        gameManager.instance.playerScript.gravityValue *=2f;
        yield return new WaitForSeconds(0.03f);
        gameManager.instance.playerScript.gravityValue = gameManager.instance.playerScript.gravityValueOG;
        gameManager.instance.playerScript.isSameWall = false;
    }
}
