using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingTrigger : MonoBehaviour
{
    public int        layerToFall;
    public GameObject fallingGuy = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            fallingGuy = other.gameObject;
            other.gameObject.layer = layerToFall;
            StartCoroutine(resetLayer());
        }
    }

    IEnumerator resetLayer()
    {
        yield return new WaitForSeconds(.8f);
        fallingGuy.layer = 0;
    }
}
