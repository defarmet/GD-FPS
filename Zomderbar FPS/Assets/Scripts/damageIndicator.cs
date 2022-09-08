using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageIndicator : MonoBehaviour
{
    public int destroyTime;

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
