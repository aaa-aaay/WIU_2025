using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : Item
{
    [HideInInspector]public int damage; 
    [HideInInspector]public int skillDamage;
    public int skillCost;
    private GameObject skillprefab;
    private Vector3 spawnOffset;
    private Vector3 positionoffset;
    private Quaternion rotationoffset;
    private PlayerStats stats;

    private BoxCollider weaponCollider;
    private List<GameObject> hasDealtDamage = new List<GameObject>();
    private void Start()
    {
        base.Start();

        if (itemSo is WeaponSO weapon) 
        {
            damage = weapon.damage;
            skillDamage = weapon.skillDamage;
            skillCost = weapon.magicCost;
            skillprefab = weapon.MagicEffect;
            spawnOffset = weapon.magicSpawnOffset;
            positionoffset = weapon.handTransformOffset;
            rotationoffset = weapon.handTransformRotation;
        }
        weaponCollider = GetComponent<BoxCollider>();
        weaponCollider.enabled = false;

    }

    public void UseWeaponAttack()
    {
        weaponCollider.enabled = true;
        hasDealtDamage.Clear();
    }
    public void DisableWeaponAttack()
    {
        weaponCollider.enabled = false;
    }
    public void UseWeaponSkill()
    {
        if(skillprefab != null) {
            if(stats != null) {
                Debug.Log("player stats found for magic"); 
                if (stats.UseMana(skillCost)) {
                    Instantiate(skillprefab, transform.position - spawnOffset, Quaternion.identity);
                } 
            }

        }
                //spawn the effect at a position at the player
    }
    public void SetWeaponPosition(Transform transform, PlayerStats stats)
    {
        this.stats = stats;
        gameObject.transform.SetParent(transform,false);
        gameObject.transform.localPosition = positionoffset;
        gameObject.transform.localRotation = rotationoffset;
    }


    private void OnTriggerEnter(Collider other) 
    {
        EnemyHealth enemy = other.gameObject.GetComponent<EnemyHealth>();
        if(enemy != null && !hasDealtDamage.Contains(enemy.gameObject))
        {
            Debug.Log("Enemy hit");
            hasDealtDamage.Add(other.gameObject);
            enemy.TakeDamage(damage);
        }
    }
}
