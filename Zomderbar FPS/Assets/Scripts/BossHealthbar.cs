using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthbar : MonoBehaviour
{
    [SerializeField] Image HealthBar;
    [SerializeField] GameObject BossCanvas;
    private enemyAI enemyScript;
    private int hp;
    private int maxHp;

    // Start is called before the first frame update
    void Start()
    {
        enemyScript = GetComponent<enemyAI>();
        hp = enemyScript.HP;
        BossCanvas.SetActive(true);
        maxHp = hp;
    }

    // Update is called once per frame
    void Update()
    {
        hp = enemyScript.HP;
        HealthBar.fillAmount = (float)hp / (float)maxHp;
        if( hp <= 0)
        {
            BossCanvas.SetActive(false);
        }
    }
}
