using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingTrigger : MonoBehaviour
{
    public int layerToFall; //changes to this layer
    public GameObject fallingGuy = null; //to see what is in trigger
    public int layerOfObject = 0; //preserve layer of the object entering. defaults to default layer

    private void OnTriggerEnter(Collider other)
    {
        //layerOfObject = other.gameObject.layer;
        if (other.CompareTag("Player"))
        {
            fallingGuy = other.gameObject;
            other.gameObject.layer = layerToFall;
            StartCoroutine(resetLayer());
        }
    }

    
    //private void OnTriggerExit(Collider other) //seems to no longer work???
    //{
    //    if (other.CompareTag("Player"))
    //        fallingGuy = null;
    //        other.gameObject.layer = 0;
    //}

    IEnumerator resetLayer() //temp fix until I figure it out
    {
        yield return new WaitForSeconds(.8f);
        fallingGuy.layer = layerOfObject;
    }
}
