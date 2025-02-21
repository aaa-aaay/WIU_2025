using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public SkillSO skill;
    private SkillTreeUI skillTreeUI;

    private void Start()
    {
        skillTreeUI = FindObjectOfType<SkillTreeUI>();  // find UI Manager
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        skillTreeUI.ShowSkillInfo(skill);
    }

    public void UpdateSkillUI(bool ableToUnlock)
    {
        Transform skillImageTransform = transform.Find("Skill Image"); // Adjust based on hierarchy

        if (skillImageTransform != null)
        {
            Image skillIcon = skillImageTransform.GetComponent<Image>();
            if (skillIcon != null)
            {
                skillIcon.color = ableToUnlock ? Color.white : Color.gray;
            }
        }
    }
}
