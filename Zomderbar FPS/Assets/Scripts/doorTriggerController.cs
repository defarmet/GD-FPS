using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorTriggerController : MonoBehaviour
{
    public Animator animObject; //a specific animation to follow on an object

    public bool openTrigger;
    public bool closeTrigger;

    public bool Block = false; //lock away trigger to open
    public Collider theDoor; // to temporarily deactivate collision;

    public string openObject;
    public string closeObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            if (openTrigger)
            {
                theDoor.enabled = false;
                animObject.Play(openObject, 0, 0.0f);
                if (Block)
                    gameObject.SetActive(false); //lock away the trigger to open door
            }
            else if (closeTrigger)
            {
                theDoor.enabled = true;
                animObject.Play(closeObject, 0, 0.0f);
                if (Block)
                    gameObject.SetActive(false); //lock away the trigger to open door
            }
        }
    }


}
