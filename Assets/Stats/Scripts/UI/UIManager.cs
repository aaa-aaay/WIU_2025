using UnityEngine;

public class SkillTreeUIManager : MonoBehaviour
{
    public GameObject skillTreePanel; // Assign in Inspector
    public MonoBehaviour playerController; // Assign your player controller script

    private bool isSkillTreeOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Press '1' to toggle
        {
            ToggleSkillTree();
        }
    }

    public void ToggleSkillTree()
    {
        isSkillTreeOpen = !isSkillTreeOpen;
        skillTreePanel.SetActive(isSkillTreeOpen);

        if (isSkillTreeOpen)
        {
            OpenUI();
        }
        else
        {
            CloseUI();
        }
    }

    private void OpenUI()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (playerController != null)
        {
            playerController.enabled = false; // Disable player movement
        }
    }

    private void CloseUI()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (playerController != null)
        {
            playerController.enabled = true; // Re-enable player movement
        }
    }
}
