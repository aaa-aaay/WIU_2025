using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField]
    private Animator playerAnim;

    [SerializeField]
    private GameObject sword;

    private PlayerInven invetroy;
    private bool isEquipped = false;  

    public bool isAttacking;
    public float timeSinceAttack;
    public int currentAttack = 0;

    private void Awake()
    {
        invetroy = GetComponentInChildren<PlayerInven>();
        invetroy.OnWeaponUpdated += SwitchWeapon;
    }
    private void Start()
    {



    }
        


    private void Update()
    {
        timeSinceAttack += Time.deltaTime;
        Attack();
        if (Input.GetKeyDown(KeyCode.R) && sword != null)
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
        Debug.Log("Weapon activated");
    }

    public void DeactivateWeapon()
    {
        sword.SetActive(false);
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
        //Sword.GetComponentInChildren<DamageDealer>().StartDealDamage();
        if (sword == null) Debug.Log("sowrd is null???");
        sword.GetComponent<Weapon>().UseWeaponAttack();

    }

    public void EndDealDamage()
    {
        sword.GetComponent<Weapon>().DisableWeaponAttack();
        //Sword.GetComponentInChildren<DamageDealer>().EndDealDamage();
    }

    public void ResetAttack()
    {
        isAttacking = false;
    }

    public void SwitchWeapon(Sprite placeholder, GameObject newWeapon)
    {
        if(newWeapon == null) return;

        sword.SetActive(false);
        sword = newWeapon;
        if(isEquipped)
            newWeapon.SetActive(true);
        else
        {
            newWeapon.SetActive(false);
        }

    }

}
