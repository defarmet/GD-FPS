using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maskObj : MonoBehaviour
{
    public GameObject[] MaskedObj;
    void Start()
    {
        for (int  i = 0; i < MaskedObj.Length; i++)
            MaskedObj[i].GetComponent<MeshRenderer>().material.renderQueue = 3002;
    }
}
