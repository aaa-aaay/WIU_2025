using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField]
    private Animator playerAnim;

    [SerializeField]
    private GameObject sword;
    [SerializeField]
    private GameObject HipSword;
    [SerializeField]
    private GameObject Sword;
    private bool isEquipped = false;  

    public bool isAttacking;
    public float timeSinceAttack;
    public int currentAttack = 0;



    private void Update()
    {
        timeSinceAttack += Time.deltaTime;
        Attack();
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleWeapon();
        }
        Attack();
    }

    private void ToggleWeapon()
    {
        // Toggle the equipped state
        isEquipped = !isEquipped;

        if (isEquipped)
        {
            Debug.Log("Equipping weapon");
            playerAnim.ResetTrigger("sheathWeapon");
            playerAnim.SetTrigger("equip");
        }
        else
        {
            Debug.Log("Sheathing weapon");
            playerAnim.ResetTrigger("equip");
            playerAnim.SetTrigger("sheathWeapon");
        }
    }

    public void ActivateWeapon()
    {
        sword.SetActive(true);
        HipSword.SetActive(false);
        Debug.Log("Weapon activated");
    }

    public void DeactivateWeapon()
    {
        sword.SetActive(false);
        HipSword.SetActive(true);
        Debug.Log("Weapon deactivated");
    }

    public void Attack()
    {

        if (Input.GetMouseButton(0) && timeSinceAttack > 0.8f)
        {
            if (!isEquipped)
                return;

            currentAttack++;
            isAttacking = true;

            if (currentAttack > 3)
                currentAttack = 1;

            //reset
            if (timeSinceAttack > 1.0f)
                currentAttack = 1;

            //call anim
            playerAnim.SetTrigger("attack" + currentAttack);

            timeSinceAttack = 0;

        }
        
    }
    public void StartDealDamage()
    {
        Sword.GetComponentInChildren<DamageDealer>().StartDealDamage();
    }

    public void EndDealDamage()
    {
        Sword.GetComponentInChildren<DamageDealer>().EndDealDamage();
    }

    public void ResetAttack()
    {
        isAttacking = false;
    }

}
