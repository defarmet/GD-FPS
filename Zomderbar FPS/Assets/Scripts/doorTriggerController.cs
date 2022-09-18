using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorTriggerController : MonoBehaviour
{
    public Animator animObject;

    public bool openTrigger;
    public bool closeTrigger;

    public bool Block = false;

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
                 * Lock away trigger to open
                 */
                if (Block)
                    gameObject.SetActive(false);
            } else if (closeTrigger) {
                theDoor.enabled = true;
                animObject.Play(closeObject, 0, 0.0f);
                if (Block)
                    gameObject.SetActive(false);
            }
        }
    }
}
