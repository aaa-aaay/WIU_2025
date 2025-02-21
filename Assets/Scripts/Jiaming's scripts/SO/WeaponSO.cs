using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Weapon", menuName = "Items/Weapon")]
public class WeaponSO : ItemSO
{
    [Header("Weapon properties")]
    public int damage;
    public int skillDamage;
    public Vector3 handTransformOffset;
    public Quaternion handTransformRotation;

    [Header("equipment limitations")]
    public int healthRequiement;
    public int speedRequirement;
    public int defenseRequirement;

    [Header("Magic properties")]
    public GameObject MagicEffect = null;
    public Vector3 magicSpawnOffset;
    public float magicCDTime;
    public int magicCost;
}
