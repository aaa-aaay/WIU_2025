using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    [HideInInspector]public float damage; 
    [HideInInspector]public float skillDamage;
    [HideInInspector] private GameObject skillprefab;
    [HideInInspector] private Vector3 spawnOffset;
    private void Start()
    {
        base.Start();

        if (itemSo is WeaponSO weapon) 
        {
            damage = weapon.damage;
            skillDamage = weapon.skillDamage;
            skillprefab = weapon.MagicEffect;
            spawnOffset = weapon.magicSpawnOffset;
        }

    }

    public void UseWeaponAttack()
    {
        //enable collier?
    }
    public void UseWeaponSkill()
    {
        if(skillprefab != null) {
            Instantiate(skillprefab, transform.position - spawnOffset, Quaternion.identity);
        }

        //spawn the effect at a position at the player
    }
}
