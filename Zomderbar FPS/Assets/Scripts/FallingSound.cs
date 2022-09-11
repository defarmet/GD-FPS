using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSound : MonoBehaviour
{
    public AudioSource fallSound;

    private void OnTriggerEnter(Collider other)
    {
        fallSound.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        fallSound.Stop();
    }
}
