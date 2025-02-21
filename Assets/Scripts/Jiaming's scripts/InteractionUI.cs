using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    [SerializeField] GameObject pickupPanel;
    [SerializeField] GameObject talkToShopPanel;
    [SerializeField] PlayerInven player;
    [SerializeField] Shop shop;

    private void Start()
    {
        player.ItemInRange += showPickUpPanel; 
        player.ItemLeftRange += HidePickUpPanel;
        shop.OnEnterShop += showTalkShopPanel;
        shop.OnExitShop += hideTalkShopPanel;
        pickupPanel.SetActive(false);
        talkToShopPanel.SetActive(false);
    }
        


    public void showPickUpPanel()
    {
        pickupPanel.SetActive(true);
    }
    public void HidePickUpPanel()
    {
        pickupPanel.SetActive(false);
    }

    public void showTalkShopPanel()
    {
        talkToShopPanel.SetActive(true);
    }
    public void hideTalkShopPanel()
    {
        talkToShopPanel.SetActive(false);
    }
}
