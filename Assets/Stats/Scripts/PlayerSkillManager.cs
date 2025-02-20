using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillManager : MonoBehaviour
{
    private PlayerStats playerStats;
    private SkillTreeUI skillTreeUI;
    private SkillSO selectedSkill;
    private SkillButton selectedButton;

    private Dictionary<SkillSO, int> unlockedSkills = new Dictionary<SkillSO, int>(); // tracks the upgrade level (currUnlocks)
    private Dictionary<SkillSO, bool> unlockedStatus = new Dictionary<SkillSO, bool>(); // tracks if skill is unlocked

    [SerializeField] private List<SkillSO> availableSkills = new List<SkillSO>(); 

    private void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        skillTreeUI = FindObjectOfType<SkillTreeUI>();

        if (availableSkills == null || availableSkills.Count == 0)
        {
            Debug.LogError("availableSkills list is null or empty!");
            return;
        }

        InitializeSkills();
    }

    private void InitializeSkills()
    {

        foreach (var skill in availableSkills)
        {
            if (skill == null)
            {
                Debug.LogError("Null skill detected in availableSkills!");
                continue;
            }

            unlockedSkills[skill] = 0;  // initialize skills with no upgrades
            unlockedStatus[skill] = false;
            if (skill.upgradeType == SkillSO.UpgradeType.Defence || skill.upgradeType == SkillSO.UpgradeType.MaxHealth || skill.upgradeType == SkillSO.UpgradeType.Strength)
            {
                unlockedStatus[skill] = true; // always start first nodes unlocked
            }
            else
            {
                unlockedStatus[skill] = false; // start as locked
            }

        }
        RefreshSkillGreying();
    }

    public void TryUnlockSkill()
    {
        if (selectedSkill == null)
        {
            Debug.Log("No skill selected.");
            return;
        }

        if (CanUnlockSkill())
        {
            unlockedSkills[selectedSkill]++; // add currUnlock
            unlockedStatus[selectedSkill] = true;
            ApplySkillUpgrade(selectedSkill); // apply upgrades

            Debug.Log($"{selectedSkill.skillName} has been unlocked!");
            skillTreeUI.ShowSkillInfo(selectedSkill);
        }
        else
        {
            Debug.Log("Skill cannot be unlocked.");
        }

        RefreshSkillGreying();
    }

    public bool CanUnlockSkill()
    {
        if (selectedSkill == null)
        {
            Debug.Log("No skill selected.");
            return false;
        }
        if (unlockedSkills[selectedSkill] < selectedSkill.maxUnlocks)
        {
            if (playerStats.HasEnoughGold(selectedSkill.goldRequired))
            {
                // Check if all previous nodes are unlocked
                if (selectedSkill.prevNodes.Count != 0)
                {
                    foreach (var prevSkill in selectedSkill.prevNodes)
                    {
                        if (/*!unlockedStatus.ContainsKey(prevSkill) || !unlockedStatus[prevSkill]*/ unlockedSkills[prevSkill] < 1)
                            return false; // a previous skill is locked, cannot unlock this skill
                    }
                }
                return true; // if player has enough gold and previous nodes are unlocked
            }
            return false; // not enough gold
        }
        return false; //no more unlocks
    }

    private void ApplySkillUpgrade(SkillSO skill)
    {
        playerStats.UpgradeStat(skill);
        playerStats.UseGold(skill.goldRequired);
    }

    public void SetSelectedSkill(SkillSO skill)
    {
        selectedSkill = skill;
    }

    // return currUnlock
    public int GetCurrentUnlockLevel(SkillSO skill)
    {
        return unlockedSkills.ContainsKey(skill) ? unlockedSkills[skill] : 0;
    }

    // return locked status
    public bool IsSkillUnlocked(SkillSO skill)
    {
        return unlockedStatus.ContainsKey(skill) && unlockedStatus[skill];
    }

    private void RefreshSkillGreying()
    {
        foreach (var skill in availableSkills)
        {
            selectedSkill = skill;
            selectedButton = FindSkillButton(selectedSkill);
            if (selectedButton != null)
            {
                selectedButton.UpdateSkillUI(CanUnlockSkill());
            }
        }
    }

    private SkillButton FindSkillButton(SkillSO skill)
    {
        SkillButton[] allSkillButtons = FindObjectsOfType<SkillButton>();
        foreach (var button in allSkillButtons)
        {
            if (button.skill == skill) // Match the SkillSO
            {
                return button;
            }
        }
        return null;
    }
}
