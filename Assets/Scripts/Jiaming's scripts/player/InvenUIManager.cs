
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InvenUIManager : MonoBehaviour
{
    [SerializeField] private PlayerInven playerIven;
    [SerializeField] GameObject potionPanel;
    [SerializeField] GameObject weaponPanel;

    private TMP_Text potionCount;
    private Image potionImage;
    private Image weaponImage;

    private void Start()
    {
        potionCount = potionPanel.GetComponentInChildren<TMP_Text>();

        potionImage = potionPanel.GetComponentsInChildren<Image>(true)
                         .FirstOrDefault(img => img.gameObject != potionPanel);

        weaponImage = weaponPanel.GetComponentsInChildren<Image>(true)
                 .FirstOrDefault(img => img.gameObject != weaponPanel);


        potionPanel.SetActive(false);
        weaponPanel.SetActive(false);

        playerIven.OnInventoryUpdated += UpdatePotionUI;
        playerIven.OnWeaponUpdated += UpdateWeaponUI;

    }


    private void UpdatePotionUI(Sprite img = null , int count = 0)
    {
        if(count <= 0)
        {
            potionPanel.SetActive(false);
            return;
        }
        potionPanel.SetActive(true);
        potionImage.sprite = img;
        potionCount.text = count.ToString();

    }
    private void UpdateWeaponUI(Sprite img = null)
    {
        if(img == null)
        {
            weaponPanel.SetActive(false);
            return;
        }
        weaponPanel.SetActive(true);
        weaponImage.sprite = img;
    }
}
