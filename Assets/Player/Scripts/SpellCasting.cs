using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCasting : MonoBehaviour
{
    
    private PlayerInven playerInven;
    private PlayerStats stats;


    private void Start()
    {
        stats = GetComponentInChildren<PlayerStats>();
        playerInven = GetComponentInChildren<PlayerInven>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(playerInven.currentWeapon != null)
            {
                playerInven.currentWeapon.UseWeaponSkill();
            }

        }
    }
}
