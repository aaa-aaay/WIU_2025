using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InvenUIManager : MonoBehaviour
{
    [SerializeField] GameObject potionPanel;
    [SerializeField] GameObject WeaponPanel;

    private TMP_Text potionCount;
    private Image potionImage;
    private Image WeaponImage;

    private void Start()
    {
        potionCount = potionPanel.GetComponentInChildren<TMP_Text>();

        potionImage = potionPanel.GetComponentsInChildren<Image>(true)
                         .FirstOrDefault(img => img.gameObject != potionPanel);

        WeaponImage = WeaponPanel.GetComponentsInChildren<Image>(true)
                 .FirstOrDefault(img => img.gameObject != WeaponPanel);


        potionPanel.SetActive(false);
        WeaponPanel.SetActive(false);

    }


    private void UpdateUI()
    {
    }
}
