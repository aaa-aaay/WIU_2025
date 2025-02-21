using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 100;
    private BossAI bossAI;
    private MonkeyAI monkeyAI;
    private FoxAI foxAI;

    private void Start()
    {
        bossAI = GetComponent<BossAI>();
        monkeyAI = GetComponent<MonkeyAI>();
        foxAI = GetComponent<FoxAI>();
    }


    public void TakeDamage(int damage)
    {
        health -= damage;
        if (bossAI != null)
        {
            bossAI.takeDamage();
        }
        else if (monkeyAI != null)
        {
            monkeyAI.takeDamage();
        }
        else if (foxAI != null)
        {
            foxAI.takeDamage();
        }
        if (health <= 0)
        {
            if (bossAI != null)
            {
                bossAI.death();
            }
            else if (monkeyAI != null)
            {
                monkeyAI.death();
            }
            else if (foxAI != null)
            {
                foxAI.death();
            }
        }
    }


}
