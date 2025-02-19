using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skill Tree/Skill")]
public class SkillSO : ScriptableObject
{
    public enum UpgradeType
    {
        Strength,
        Speed,
        Defence,
        MaxHealth,
        MaxMana,
        Gold,
        Heal,
        Barrier
    }


    [Header ("Skill Info")]
    public string skillName;
    public string description;

    [Header("Upgrade Effects")]
    public UpgradeType upgradeType;
    public int statChange;

    [Header("Unlock System")]
    public int goldRequired;
    public int maxUnlocks;
    public int currUnlocks;
    public bool isUnlocked;

    [Header("Previous Nodes")]
    public List<SkillSO> prevNodes; // list of all prev nodes

    public bool CanUnlock(PlayerStats playerStats)
    {
        if (currUnlocks >= maxUnlocks || !playerStats.HasEnoughGold(goldRequired)) return false; // if max unlocked or not enough gold, return false and stay locked

        foreach (SkillSO skill in prevNodes)
        {
            if (!skill.isUnlocked) return false; // if previous nodes not unlocked, return false and stay locked
        }

        return true; // if node is unlockable
    }

}
