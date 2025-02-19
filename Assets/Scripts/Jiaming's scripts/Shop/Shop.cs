
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class Shop : MonoBehaviour
{

    [SerializeField] private GameObject shopCanvas;
    [SerializeField] private List<ShopSlot> shopSlots;
    [SerializeField] private List<ShopSlot> playerSlot;
    [SerializeField] private List<Item> shopItems;

    private List<Item> items;

    private void Start()
    {
        shopCanvas.SetActive(false);
    }
    private void OnTriggerStay(Collider other)
    {
        PlayerInven player = other.gameObject.GetComponentInChildren<PlayerInven>();
        if (player != null)
        {
            if (Input.GetKey(KeyCode.E))
            {

                //openShop
                shopCanvas.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                items = player.GetPlayerInventory();
                DisplayInventory();
                DisplayShop();

            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        PlayerInven player = other.gameObject.GetComponentInChildren<PlayerInven>();
        if (player != null)
        {
            shopCanvas.SetActive(false);
        }
    }


    private void DisplayInventory()
    {

        foreach (var slot in playerSlot)
        {
            slot.ClearInfo();
        }
        
        items = items.Where(item => item != null).ToList();
        
        var groupedItems = items
            .GroupBy(item => item.itemName)
            .Select(group => new
            {
                Item = group.First(), // Use the first item for UI display
                Count = group.Count()  // Count how many exist
            }).ToList();

        for (int i = 0; i < groupedItems.Count && i < playerSlot.Count; i++)
        {
            playerSlot[i].SetInfo(
                groupedItems[i].Count,   
                groupedItems[i].Item.uiImage,
                groupedItems[i].Item.itemName,
                groupedItems[i].Item.discription,
                groupedItems[i].Item.price
            );
        }
    }
    private void DisplayShop()
    {
        foreach (var slot in shopSlots)
        {
            slot.ClearInfo();
        }

        shopItems = shopItems.Where(shopItems => shopItems != null).ToList();

        var groupedItems = shopItems
            .GroupBy(shopItems => shopItems.itemName)
            .Select(group => new
            {
                Item = group.First(), // Use the first item for UI display
                Count = group.Count()  // Count how many exist
            }).ToList();

        for (int i = 0; i < groupedItems.Count && i < shopSlots.Count; i++)
        {
            shopSlots[i].SetInfo(
                groupedItems[i].Count,
                groupedItems[i].Item.uiImage,
                groupedItems[i].Item.itemName,
                groupedItems[i].Item.discription,
                groupedItems[i].Item.price
            );
        }
    }

}

