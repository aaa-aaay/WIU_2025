using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackState : MonoBehaviour
{
    private Animator _animator;
    private PlayerInput _playerInput;
    private InputActionAsset _inputActions;


    private bool isAttacking = false;
    private float attackCooldown = 0.5f;
    private float lastAttackTime;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _playerInput = GetComponent<PlayerInput>();
        _inputActions = _playerInput.actions;
    }

    private void Update()
    {
        if (_inputActions["Attack"].WasPressedThisFrame() && !isAttacking && Time.time >= lastAttackTime + attackCooldown)
        {
            StartCoroutine(PerformAttack());
        }
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        _animator.SetTrigger("attack");

        // Wait for the attack animation to finish
        yield return new WaitForSeconds(GetCurrentAnimationLength());

        isAttacking = false;
    }

    private float GetCurrentAnimationLength()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.length / stateInfo.speed;
    }
}
