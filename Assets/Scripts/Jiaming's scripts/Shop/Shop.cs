
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using System;

public class Shop : MonoBehaviour
{

    [SerializeField] private GameObject shopCanvas;
    [SerializeField] private List<ShopSlot> shopSlots;
    [SerializeField] private List<ShopSlot> playerSlot;
    [SerializeField] private List<Item> shopItems;
    private PlayerInven player;
    private PlayerStats playerStats;
    private PlayerControllerActual pc;

    private List<Item> items;
    private bool shopPanelOpen = false; // Add a toggle state
    private float interactionCooldown = 0.2f; // Add a cooldown duration
    private float lastInteractionTime = 0f; // Track the last interaction time
    public event Action OnEnterShop;
    public event Action OnExitShop;

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

    private void Update()
    {
    }


    private void OnTriggerStay(Collider other)
    {
        if (player == null)
            player = other.GetComponentInChildren<PlayerInven>();

        if (playerStats == null)
            playerStats = other.GetComponentInParent<PlayerStats>();

        if (pc == null)
            pc = other.GetComponentInParent<PlayerControllerActual>();

        if (player != null && pc != null && Time.time - lastInteractionTime > interactionCooldown)
        {
            OnEnterShop?.Invoke();
            if (Input.GetKey(KeyCode.E))
            {
                lastInteractionTime = Time.time;
                shopPanelOpen = !shopPanelOpen;

                if (shopPanelOpen)
                {
                    // Open Shop

                    pc.enabled = false;
                    shopCanvas.SetActive(true);
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    items = player.GetPlayerInventory();
                    DisplayInventory();
                    DisplayShop();
                }
                else
                {

                    pc.enabled = true;
                    // Close Shop
                    shopCanvas.SetActive(false);
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;


                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Ensure everything is closed when the player leaves the trigger area
        if (other.GetComponentInChildren<PlayerInven>() == player)
        {
            OnExitShop?.Invoke();
            shopPanelOpen = false;
            shopCanvas.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            if (pc != null)
                pc.enabled = true;

            player = null;
            playerStats = null;
            pc = null;
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

