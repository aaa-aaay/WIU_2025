
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
    private PlayerInven player;
    private PlayerStats playerStats;

    private List<Item> items;

    private void OnEnable()
    {
        shopCanvas.SetActive(true);
    }

    private void Start()
    {
        shopCanvas.SetActive(false);

        foreach (var shopSlot in shopSlots)
        {
            shopSlot.BuyingItem += ItemBought;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(player == null)
         player = other.gameObject.GetComponentInChildren<PlayerInven>();

        if(playerStats == null)
            playerStats = other.GetComponentInParent<PlayerStats>();

        if (player != null)
        {
            if (Input.GetKey(KeyCode.E))
            {
                if(shopCanvas.activeSelf)
                {
                    shopCanvas.SetActive(false);
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
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
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
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

    private void ItemBought(string name, int price)
    {
        if(!playerStats.HasEnoughGold(price)) return;
        for (int i = shopItems.Count - 1; i >= 0; i--)
        {
            Item item = shopItems[i];
            if (item.itemName == name)
            {
                if (player != null)
                {
                    item.PickUp(player.gameObject);
                    shopItems.RemoveAt(i);
                    items.Add(item);
                    DisplayShop();
                    DisplayInventory();
                    player.SetPlayerInventory(item,true);
                    playerStats.UseGold(price);
                    break;
                }
                else
                {
                    Debug.Log("player is null");
                }
            }
        }
    }



}

