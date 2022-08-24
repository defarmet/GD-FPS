using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorDrop : MonoBehaviour
{

    [SerializeField] int enemiesToKill;
    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    void Update()
    {
        if (enemiesToKill <= gameManager.instance.enemyKilled)
        {
            gameObject.SetActive(false);
        }

        else
        {
            gameObject.SetActive(true);
        }

        
    }
}
