using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunStats : ScriptableObject
{
    [Range(0.1f, 5)] public float shootRate;
    [Range(1, 30)] public int shootDist;
    [Range(1, 10)] public int shootDmg;
}
