using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeRandomSound : MonoBehaviour
{
    private AudioSource source;
    [SerializeField] AudioClip[] arrayOfSounds;
    [SerializeField] float volume;
    void Awake()
    {
        source = GetComponent<AudioSource>();
        source.clip = arrayOfSounds[Random.Range(0, arrayOfSounds.Length)];
        source.PlayOneShot(source.clip, volume);
    }
}
