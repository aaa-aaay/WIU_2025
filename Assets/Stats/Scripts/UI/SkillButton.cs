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
}
