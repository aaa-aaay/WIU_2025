using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AlertBehaviour : StateMachineBehaviour
{
    private BossAI boss;
    private MonkeyAI monkey;
    private FoxAI fox;
    public States setState;
    public whichAI currentAI;
    private GameObject enemyObj;
    private void Awake()
    {

        if (currentAI == whichAI.BOSS)
        {
            enemyObj = GameObject.Find("Penelope(Clone)");
            boss = enemyObj.GetComponent<BossAI>();
            if (boss == null)
            {
                Debug.LogWarning("BossAI instance not found!");
            }
        }
        else if (currentAI == whichAI.MONKEY)
        {
            enemyObj = GameObject.Find("Monkey(Clone)");
            monkey = enemyObj.GetComponent<MonkeyAI>();
            if (boss == null)
            {
                Debug.LogWarning("MonkeyAI instance not found!");
            }
        }
        else if (currentAI == whichAI.FOX)
        {
            enemyObj = GameObject.Find("Fox(Clone)");
            fox = enemyObj.GetComponent<FoxAI>();
            if (fox == null)
            {
                Debug.LogWarning("foxAI instance not found!");
            }
        }

    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (currentAI == whichAI.BOSS)
        {
            boss.resetAnimationBools();
            boss.setBossState(setState);
        }
        else if (currentAI == whichAI.MONKEY)
        {
            monkey.resetAnimationBools();
            monkey.setBossState(setState);
        }
        else if (currentAI == whichAI.FOX)
        {
            fox.resetAnimationBools();
            fox.setBossState(setState);
        }

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
