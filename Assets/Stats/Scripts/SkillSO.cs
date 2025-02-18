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
        Mana,
        Gold,
        Heal,
        Barrier
    }

    public string skillName;
    public string description;

    public UpgradeType upgradeType;
    public int statChange;
    
}
