using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class togglePlatforms : MonoBehaviour
{
    [SerializeField] GameObject boss;
    [SerializeField] GameObject[] platforms;
    private enemyAI bossScript;
    // Start is called before the first frame update
    void Start()
    {
        bossScript = boss.GetComponent<enemyAI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(bossScript.HP <= 0)
        {
            for (int i = 0; i < platforms.Length; ++i)
                platforms[i].SetActive(true);
        }
    }
}
