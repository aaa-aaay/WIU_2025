using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSO : ScriptableObject
{
    public enum SkillType
    {
        Strength,
        Speed,
        MaxHealth,
        Heal,
        Barrier
    }

    public string skillName;
    public string description;

    public SkillType skillType;
    public int statIncrease;
    
}
