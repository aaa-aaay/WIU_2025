
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InvenUIManager : MonoBehaviour
{
    [SerializeField] private PlayerInven playerIven;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] GameObject potionPanel;
    [SerializeField] GameObject weaponPanel;
    [SerializeField] GameObject moneyPanel;


    private TMP_Text potionCount;
    private Image potionImage;
    private Image weaponImage;
    private TMP_Text moneyText;

    private void Awake()
    {

        potionCount = potionPanel.GetComponentInChildren<TMP_Text>();

        potionImage = potionPanel.GetComponentsInChildren<Image>(true)
                         .FirstOrDefault(img => img.gameObject != potionPanel);

        weaponImage = weaponPanel.GetComponentsInChildren<Image>(true)
                 .FirstOrDefault(img => img.gameObject != weaponPanel);

        potionPanel.SetActive(false);
        weaponPanel.SetActive(false);

        moneyText = moneyPanel.GetComponentInChildren<TMP_Text>();
        playerStats.OnMoneyAmtChanged += UpdateMoneyCount;
        playerIven.OnInventoryUpdated += UpdatePotionUI;
        playerIven.OnWeaponUpdated += UpdateWeaponUI;
        playerStats.OnMoneyAmtChanged += UpdateMoneyCount;
    }

    private void Start()
    {









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
    private void UpdateWeaponUI(Sprite img, GameObject wepaonGO)
    {
        Debug.Log("updating weapon null");
        if(img == null)
        {
            Debug.Log("image is null");
            weaponPanel.SetActive(false);
            return;
        }
        weaponPanel.SetActive(true);
        weaponImage.sprite = img;   
    }

    private void UpdateMoneyCount(int amt)
    {
        moneyText.text = amt.ToString();
    }
}
