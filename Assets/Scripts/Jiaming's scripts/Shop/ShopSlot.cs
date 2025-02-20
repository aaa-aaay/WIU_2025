
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;
using System;

public class ShopSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TMP_Text amountTxt;
    private Image image;
    [SerializeField] private GameObject descriptionPanel;

    private string desc, itemName;
    private int price;
    public event Action<string, int> BuyingItem;
    private bool hovering = false;


    private void Awake()
    {
        amountTxt = GetComponentsInChildren<TMP_Text>(true)
    .FirstOrDefault(txt => txt.gameObject != gameObject);

        image = GetComponentsInChildren<Image>(true)
            .FirstOrDefault(img => img.gameObject != gameObject);

        amountTxt.text = string.Empty;
        image.gameObject.SetActive(false);
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && hovering)
        {
            BuyingItem?.Invoke(itemName, price);
        }

    }

    public void SetInfo(int amount, Sprite img, string name, string description, int price)
    {
        image.gameObject.SetActive (true);
        image.sprite = img;
        amountTxt.text = amount.ToString();

        desc = description;
        itemName = name;
        this.price = price;

    }
    public void ClearInfo()
    {
        amountTxt.text = string.Empty;

        image.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!image.gameObject.activeSelf) return;
        descriptionPanel.SetActive(true);
        DescPanel descPanel = descriptionPanel.GetComponent<DescPanel>();
        descPanel.ChangeDescPanelInfo(itemName, desc, price);
        descriptionPanel.transform.position = transform.position;

        hovering = true;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
        descriptionPanel.SetActive(false);
    }
}
