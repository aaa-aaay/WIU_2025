using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillTreeUI : MonoBehaviour
{
    public PlayerStats playerStats;
    private PlayerSkillManager skillManager;
    public TextMeshProUGUI skillNameText;
    public TextMeshProUGUI skillDescriptionText;
    public TextMeshProUGUI maxcurrUnlocksText;
    public TextMeshProUGUI goldRequiredText;
    public Button unlockButton;

    private bool isPanelOpen = false;

    private void Start()
    {
        skillManager = FindObjectOfType<PlayerSkillManager>();
    }

    public void ShowSkillInfo(SkillSO skill)
    {
        skillManager.SetSelectedSkill(skill); // set selected skill

        // get skill info
        int currUnlocks = skillManager.GetCurrentUnlockLevel(skill);
        bool isUnlocked = skillManager.IsSkillUnlocked(skill);

        // update UI
        skillNameText.text = skill.skillName;
        skillDescriptionText.text = skill.description;
        goldRequiredText.text = $"Gold Required: {skill.goldRequired}";
        maxcurrUnlocksText.text = $"Unlocks Left: {skill.maxUnlocks - currUnlocks}";

        // enable unlock button if it can be unlocked
        unlockButton.interactable = skillManager.CanUnlockSkill();
    }
}
