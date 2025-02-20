using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCasting : MonoBehaviour
{
    public Weapon weapon;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            weapon.UseWeaponSkill();
        }
    }
}
