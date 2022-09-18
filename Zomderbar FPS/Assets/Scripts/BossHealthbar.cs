using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthbar : MonoBehaviour
{
    [SerializeField] Image      HealthBar;
    [SerializeField] GameObject BossCanvas;
                     enemyAI    enemyScript;
                     int        hp;
                     int        maxHp;

    void Start()
    {
        enemyScript = GetComponent<enemyAI>();
        hp = enemyScript.HP;
        BossCanvas.SetActive(true);
        maxHp = hp;
    }

    void Update()
    {
        hp = enemyScript.HP;
        HealthBar.fillAmount = (float)hp / (float)maxHp;
        if (hp < 1)
            BossCanvas.SetActive(false);
    }
}
