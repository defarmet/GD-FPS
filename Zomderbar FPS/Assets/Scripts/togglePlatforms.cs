using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class togglePlatforms : MonoBehaviour
{
    [SerializeField] GameObject   boss;
    [SerializeField] GameObject[] platforms;
                     enemyAI      bossScript;
    
    void Start()
    {
        bossScript = boss.GetComponent<enemyAI>();
    }

    void Update()
    {
        if(bossScript.HP < 1) {
            for (int i = 0; i < platforms.Length; ++i)
                platforms[i].SetActive(true);
        }
    }
}
