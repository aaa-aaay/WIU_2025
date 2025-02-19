
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class Shop : MonoBehaviour
{

    [SerializeField] private GameObject shopCanvas;
    [SerializeField] private List<ShopSlot> shopSlots;
    [SerializeField] private List<ShopSlot> playerSlot;

    private List<Item> items;

    private void Start()
    {
        shopCanvas.SetActive(false);
    }
    private void OnTriggerStay(Collider other)
    {
        PlayerInven player = other.gameObject.GetComponentInChildren<PlayerInven>();
        if(player != null)
        {
            if (Input.GetKey(KeyCode.E)) {

                //openShop
                shopCanvas.SetActive(true);
                items = player.GetPlayerInventory();
                DisplayInventory();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        PlayerInven player = other.gameObject.GetComponentInChildren<PlayerInven>();
        if(player != null) {
            shopCanvas.SetActive(false);
        }
    }


    private void DisplayInventory()
    {
        Debug.Log("Player Inventory Count: " + items.Count);

        foreach (var slot in shopSlots)
        {
            slot.ClearInfo(); 
        }
        // Filter out any null or destroyed items
        foreach (var item in items)
        {
            if (item == null)
            {
                Debug.LogWarning("Null item detected before filtering.");
            }
            else if (item.gameObject == null)
            {
                Debug.LogWarning($"Destroyed item detected: {item.name}");
            }
            else
            {
                Debug.Log($"Valid item: {item.name}");
            }
        }


        items = items.Where(item => item != null).ToList();
        Debug.Log("Player Inventory Count: " + items.Count);

        //var groupedItems = items
        //    .Where(item => item != null) // Ensure no destroyed objects are processed
        //    .GroupBy(item => item.name)
        //    .Select(group => new
        //    {
        //        Item = group.First(),
        //        Count = group.Count()
        //    }).ToList();

        for (int i = 0; i < items.Count && i < shopSlots.Count; i++)
        {
            shopSlots[i].SetInfo(
                items.Count,
                items[i].uiImage,
                items[i].name,
                "testing",
                10
            );
        }
    }
}
