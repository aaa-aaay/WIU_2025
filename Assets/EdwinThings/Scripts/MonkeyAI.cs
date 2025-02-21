using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class MonkeyAI : MonoBehaviour
{
    Transform playerPos;
    private States currentState;
    [SerializeField] Animator animator;
    [SerializeField] SphereCollider attackCollider;

    // Adjustable rotation speed
    [SerializeField] float rotationSpeed = 5f;

    void Start()
    {
        GameObject obj = GameObject.Find("player");
        if (obj == null) Debug.Log("no player");
        playerPos = obj.GetComponent<Transform>();

        resetAnimationBools();
        currentState = States.CHASE;
        if (animator == null)
        {
            Debug.LogWarning("There aint no animator sir");
        }
    }
    // Update is called once per frame
    void Update()
    {
        StateMachine();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentState = States.ATTACKING;
        }
    }


    public void resetAnimationBools()
    {
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
                    Quaternion targRot = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targRot, rotationSpeed * Time.deltaTime);
                }
                if (!animator.GetBool("IsChasing"))
                {
                    animator.SetBool("IsChasing", true);
                    Debug.Log("State: CHASE");
                }
                break;

            case States.ATTACKING:
                direction = playerPos.position - transform.position;
                direction.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                if (!animator.GetBool("IsAttacking"))
                {
                    Debug.Log("State: Attack");
                    animator.SetBool("IsAttacking", true);
                }
                break;
            case States.ATTACKED:
                // TODO: Add attacked behavior
                Debug.Log("State: ATTACKED");
                break;

            case States.DEATH:
                // TODO: Add death behavior
                Debug.Log("State: DEATH");
                break;

            default:
                Debug.LogWarning("Unknown state encountered! / No state set");
                break;
        }
    }
}
