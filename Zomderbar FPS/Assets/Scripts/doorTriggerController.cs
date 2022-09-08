using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorTriggerController : MonoBehaviour
{
    /*
     * a specific animation to follow on an object
     */
    public Animator animObject;

    public bool openTrigger;
    public bool closeTrigger;

    /*
     * lock away trigger to open
     */
    public bool Block = false;
    /*
     * to temporarily deactivate collision;
     */
    public Collider theDoor;

    public string openObject;
    public string closeObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            if (openTrigger) {
                theDoor.enabled = false;
                animObject.Play(openObject, 0, 0.0f);
                /*
                 * lock away the trigger to open door
                 */
                if (Block)
                    gameObject.SetActive(false);
            } else if (closeTrigger) {
                theDoor.enabled = true;
                animObject.Play(closeObject, 0, 0.0f);
                /*
                 * lock away the trigger to open door
                 */
                if (Block)
                    gameObject.SetActive(false);
            }
        }
    }
}
