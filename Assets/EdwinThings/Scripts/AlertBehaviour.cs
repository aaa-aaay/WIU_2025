using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AlertBehaviour : StateMachineBehaviour
{
    [SerializeField] private BossAI boss;
    public States setState;

    private void Awake()
    {
        GameObject bossObject = GameObject.Find("Rumba Dancing");
        boss = bossObject.GetComponent<BossAI>();
        if (boss == null)
        {
            Debug.LogWarning("BossAI instance not found!");
        }
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("DONE");  
        boss.resetAnimationBools();
        boss.setBossState(setState);
    }
}
