using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class FoxAI : MonoBehaviour
{
    private Transform playerPos;
    private States currentState;
    [SerializeField] Animator animator;
    [SerializeField] SphereCollider attackCollider;
    private Quaternion targetRotation;
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
        Debug.Log(currentState);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            resetAnimationBools();
            currentState = States.ATTACKING;
        }
    }


    public void resetAnimationBools()
    {
        attackCollider.enabled = true;
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
                    targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
                if (!animator.GetBool("IsChasing"))
                {
                    animator.SetBool("IsChasing", true);
                }
                break;

            case States.ATTACKING:
                direction = playerPos.position - transform.position;
                direction.y = 0;
                targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                if (!animator.GetBool("IsAttacking"))
                {
                    attackCollider.enabled = false;
                    animator.SetBool("IsAttacking", true);
                }


                break;

          

            case States.VUNERABLE:
                // TODO: Add vulnerable behavior
                Debug.Log("State: VUNERABLE");
                break;

            case States.ATTACKED:

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
                break;
        }
    }
}
