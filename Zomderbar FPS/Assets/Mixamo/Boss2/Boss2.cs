using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : MonoBehaviour
{
    [SerializeField] GameObject bulletPhase2;
    [SerializeField] int enragedDamage;
    [SerializeField] int enragedSpeed;
    private enemyAI enemyScript;
    private Animator anim;
    private int hp;
    bool enraged = false;

    private void Start()
    {
        enemyScript = GetComponent<enemyAI>();
        anim = GetComponent<Animator>();
        hp = enemyScript.HP;
    }
    private void Update()
    {
        hp = enemyScript.HP;
        if(hp <= 150 && !enraged)
        {
            anim.SetBool("IsEnraged", true);
            enraged = true;
            enemyScript.SetBullet(bulletPhase2);
            enemyScript.SetBulletSpeed(0);
            enemyScript.SetBulletTime(3);
            enemyScript.SetDamage(enragedDamage);
            enemyScript.SetShootRange(6);
            enemyScript.SetShootRate(3);
            enemyScript.SetAnimBuffer(1.55f);

        }
    }
    public void SlowDown()
    {
        enemyScript.SetSpeed(0);
    }
    public void SetEnragedSpeed()
    {
        enemyScript.SetSpeed(enragedSpeed);
    }

    public void SetHpHalf()
    {
        enemyScript.HP = 200;
    }
}
