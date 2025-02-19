
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ShopSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text amountTxt;
    [SerializeField] private Image image;
    [SerializeField] private GameObject descriptionPanel;

    private void Start()
    {

        amountTxt = GetComponentsInChildren<TMP_Text>(true)
            .FirstOrDefault(txt => txt.gameObject != gameObject);

        image = GetComponentsInChildren<Image>(true)
            .FirstOrDefault(img => img.gameObject != gameObject);

        amountTxt.text = string.Empty;
        image.gameObject.SetActive(false);
        //descriptionPanel.SetActive(false);
    }

    public void SetInfo(int amount, Sprite img, string name, string Description, int price)
    {
        image.gameObject.SetActive (true);
        image.sprite = img;
        amountTxt.text = amount.ToString();
    }
    public void ClearInfo()
    {
        amountTxt.text = string.Empty;
        image.gameObject.SetActive(false);
    }
}
