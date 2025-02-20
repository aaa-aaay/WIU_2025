using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCasting : MonoBehaviour
{

    [SerializeField] private PlayerInven playerInven;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Spell cast");
            if(playerInven.currentWeapon != null)
            {
                playerInven.currentWeapon.UseWeaponSkill();
            }

        }
    }
}
