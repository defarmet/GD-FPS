using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingTrigger : MonoBehaviour
{
    public int layerToFall; //changes to this layer
    private int layerOfObject = 0; //preserve layer of the object entering. defaults to default layer

    private void OnTriggerEnter(Collider other)
    {
        layerOfObject = other.gameObject.layer;
        other.gameObject.layer = layerToFall;
    }
    private void OnTriggerExit(Collider other)
    {
        other.gameObject.layer = layerOfObject;
    }
}
