using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    [HideInInspector]public float damage; 
    [HideInInspector]public float skillDamage; 
    private void Start()
    {
        base.Start();

        if (itemSo is WeaponSO weapon) 
        {
            damage = weapon.damage;
            skillDamage = weapon.skillDamage;
        }

    }
}
