using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int startStrength;
    [SerializeField] private int startSpeed;
    [SerializeField] private int startDefence;
    [SerializeField] private int startHealth;

    private int strength;
    private int speed;
    private int defence;
    private int health;
    private int healPercentage;
    private int barrierPercentage;

    private bool unlockedHeal;
    private bool unlockedBarrier;

    public int Strength => strength;
    public int Speed => speed;
    public int Defence => defence;
    public int Health => health;
    public int MaxHealth => startHealth;
    public bool UnlockedHeal => unlockedHeal;
    public bool UnlockedBarrier => unlockedBarrier;

    private void Start()
    {
        strength = startStrength;
        speed = startSpeed;
        defence = startDefence;
        health = startHealth;
        unlockedHeal = false;
        unlockedBarrier = false;
    }

    public void UpgradeStat(SkillSO type)
    {
        switch (type.upgradeType)
        {
            case SkillSO.UpgradeType.Strength:
                strength += type.statChange;
                break;
            case SkillSO.UpgradeType.Speed:
                speed += type.statChange;
                break;
            case SkillSO.UpgradeType.Defence:
                defence += type.statChange;
                break;
            case SkillSO.UpgradeType.MaxHealth:
                startHealth += type.statChange;
                health += type.statChange;
                break;
            case SkillSO.UpgradeType.Heal:
                unlockedHeal = true;
                break;
            case SkillSO.UpgradeType.Barrier:
                unlockedBarrier = true;
                break;
        }
    }

    public void TakeDamage(int amt)
    {
        if (amt - defence > 0)
        {
            health = health - (amt - defence);
        }
    }
}
