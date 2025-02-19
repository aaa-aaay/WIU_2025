using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillTreeUI : MonoBehaviour
{
    public PlayerStats playerStats; 
    public TextMeshProUGUI skillNameText;
    public TextMeshProUGUI skillDescriptionText;
    public TextMeshProUGUI maxcurrUnlocksText;
    public TextMeshProUGUI goldRequiredText;
    public Button unlockButton;

    private SkillSO selectedSkill;

    public void ShowSkillInfo(SkillSO skill)
    {
        selectedSkill = skill;  // stores selected skill

        // update UI
        skillNameText.text = skill.skillName;
        skillDescriptionText.text = skill.description;
        goldRequiredText.text = $"Gold Required: {skill.goldRequired}";
        maxcurrUnlocksText.text = $"Unlocks Left: {skill.maxUnlocks - skill.currUnlocks}";

        // enable unlock button if it can be unlocked
        unlockButton.interactable = skill.CanUnlock(playerStats);
    }

    public void TryUnlockSkill()
    {
        if (selectedSkill == null) return;

        if (selectedSkill.CanUnlock(playerStats))
        {
            selectedSkill.isUnlocked = true;
            selectedSkill.currUnlocks++;
            playerStats.UseGold(selectedSkill);

            Debug.Log($"{selectedSkill.skillName} has been unlocked!");

            // refresh UI
            ShowSkillInfo(selectedSkill);
        }
        else
        {
            Debug.Log("Cannot unlock this skill.");
        }
    }
}
