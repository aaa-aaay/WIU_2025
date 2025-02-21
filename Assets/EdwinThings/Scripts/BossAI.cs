using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class BossAI : MonoBehaviour
{
    Transform playerPos;
    private States currentState;
    [SerializeField] Animator animator;
    [SerializeField] SphereCollider attackCollider;
    private bool shouldAttack;
    private int whichAttack;

    // Adjustable rotation speed
    [SerializeField] float rotationSpeed = 5f;

    void Start()
    {
        resetAnimationBools();
        whichAttack = 0;
        currentState = States.CHASE;
        if (animator == null)
        {
            Debug.LogWarning("There aint no animator sir");
        }

        GameObject obj = GameObject.Find("player");
        playerPos = obj.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine();
        Debug.Log(currentState);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Attack");
            currentState = States.ATTACKING;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            resetAnimationBools();
            currentState = States.CHASE;
        }
    }

    public void resetAnimationBools()
    {
        Debug.Log("RESET");
        animator.SetBool("IsChasing", false);
        animator.SetBool("IsAttacking", false);
    }

    public void setBossState(States statename)
    {
        resetAnimationBools();
        currentState = statename;
    }

    private void StateMachine()
    {
        switch (currentState)
        {
            case States.CHASE:
                Vector3 direction = playerPos.position - transform.position;
                direction.y = 0; 

                if (direction.sqrMagnitude > 0.001f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
                if (!animator.GetBool("IsChasing"))
                {
                    animator.SetBool("IsChasing", true);
                    Debug.Log("State: CHASE");
                }
                break;

            case States.ATTACKING:
                whichAttack = UnityEngine.Random.Range(0, 4);
                if (!animator.GetBool("IsAttacking"))
                {
                    Debug.Log("State: Attack");
                    animator.SetBool("IsAttacking", true);
                    Debug.Log(whichAttack); 
                    animator.SetInteger("WhichAttack", whichAttack);
                }


                break;

            case States.VUNERABLE:
                // TODO: Add vulnerable behavior
                Debug.Log("State: VUNERABLE");
                break;

            case States.ATTACKED:
                // TODO: Add attacked behavior
                Debug.Log("State: ATTACKED");
                break;

            case States.STUNNED:
                // TODO: Add stunned behavior
                Debug.Log("State: STUNNED");
                break;

            case States.DEATH:
                // TODO: Add death behavior
                Debug.Log("State: DEATH");
                break;

            default:
                Debug.LogWarning("Unknown state encountered! / No state set");
                Debug.Log(currentState);
                break;
        }
    }
}
