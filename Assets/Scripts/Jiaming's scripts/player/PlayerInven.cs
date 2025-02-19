using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInven : MonoBehaviour
{
    // can put this in a child object of the player
    private KeyCode tempKeyCode = KeyCode.E;
    private KeyCode tempKeyCode2 = KeyCode.Q;
    private KeyCode tempKeyCodeSwitchPotion = KeyCode.DownArrow;
    private KeyCode tempKeyCodeSwitchPotion2 = KeyCode.UpArrow;
    [SerializeField] private SphereCollider pickupRange;
    [SerializeField] private GameObject pickUpPanel;
    [SerializeField] private PlayerStats playerStats;

    private List<Potion> healthpotion = new List<Potion>();
    private List<Potion> mamaPotion = new List<Potion>();
    private List<Weapon> weaponList = new List<Weapon>();

    private int currentPotionDisplayed;
    private int currentWeaponDisplayed;

    private Potion currentPotion;
    private Weapon currentWeapon;

    private bool isPickingUp = false;

    public event Action<Sprite, int> OnInventoryUpdated;

    private void Start()
    {
        currentPotionDisplayed = 0;
        pickUpPanel.SetActive(false);

    }

    private void Update()
    {
        if(Input.GetKeyDown(tempKeyCodeSwitchPotion) || Input.GetKeyDown(tempKeyCodeSwitchPotion2))
        {
            if (currentPotionDisplayed == 1 && mamaPotion.Count > 0)
                currentPotionDisplayed = 2;
            else if (currentPotionDisplayed == 2 && healthpotion.Count > 0) 
                currentPotionDisplayed = 1;

            if(healthpotion.Count < 0 && healthpotion.Count < 0)
            {
                currentPotionDisplayed = 0;
            }
            UpdatePotionUI();
        }

        if (Input.GetKeyDown(tempKeyCode2) && (healthpotion.Count > 0 || mamaPotion.Count > 0))
        {
            UsePotion();
        }
    }


    private void UpdatePotionUI()
    {
        if (currentPotionDisplayed == 1)
        {
            if(healthpotion.Count == 0)
            {
                OnInventoryUpdated?.Invoke(null, healthpotion.Count);
                return;
            }


            OnInventoryUpdated?.Invoke(healthpotion.First().uiImage, healthpotion.Count);
            //update the ui with the new count;
        }    
        else if (currentPotionDisplayed == 2)
        {
            if (mamaPotion.Count == 0)
            {
                OnInventoryUpdated?.Invoke(null, mamaPotion.Count);
                return;
            }


            OnInventoryUpdated?.Invoke(mamaPotion.First().uiImage, mamaPotion.Count);
        }


        else if (currentPotionDisplayed == 0)
        {
            OnInventoryUpdated?.Invoke(null, 0);
            if (healthpotion.Count > 0 || mamaPotion.Count > 0)
            {
                if (healthpotion.Count > 0) currentPotionDisplayed = 1;
                else if (mamaPotion.Count > 0) currentPotionDisplayed = 2;
                UpdatePotionUI();
            }

        }
    }

    private void UsePotion()
    {
        if (currentPotionDisplayed == 1 && healthpotion.Count > 0)
        {
            Potion usedPotion = healthpotion.Last();
            healthpotion.Remove(usedPotion);
            Destroy(usedPotion.gameObject);

            // Switch to mana potions if no health potions are left
            if (healthpotion.Count == 0 && mamaPotion.Count > 0)
                currentPotionDisplayed = 2;
            else if (healthpotion.Count == 0 && mamaPotion.Count == 0)
                currentPotionDisplayed = 0; // No more potions

            UpdatePotionUI();
        }
        else if (currentPotionDisplayed == 2 && mamaPotion.Count > 0)
        {
            Potion usedPotion = mamaPotion.Last();
            mamaPotion.Remove(usedPotion);
            Destroy(usedPotion.gameObject);

            //playerStats.RestoreMana(usedPotion.potionStrength);
  

            // Switch to health potions if no mana potions are left
            if (mamaPotion.Count == 0 && healthpotion.Count > 0)
                currentPotionDisplayed = 1;
            else if (mamaPotion.Count == 0 && healthpotion.Count == 0)
                currentPotionDisplayed = 0; // No more potions

            UpdatePotionUI();
        }
    }


    private void OnTriggerStay(Collider other)
    {
        Item item = other.GetComponent<Item>();
        if(item != null)
        {
            pickUpPanel.SetActive(true);
            //TODO: Show the Pick UI
            if (Input.GetKey(tempKeyCode) && !isPickingUp  )
            {
                isPickingUp = true;

                if (item is Potion potion)
                {

                    if (potion.type == PotionSO.PotionType.HEALTH)
                    {
                        healthpotion.Add(potion);
                    }
                    else if (potion.type == PotionSO.PotionType.MANA)
                    {
                        mamaPotion.Add(potion);
                    }

                    UpdatePotionUI();
                }



                else if(item is Weapon)
                {
                    weaponList.Add((Weapon)item);
                }

                 
                item.PickUp(gameObject);
                pickUpPanel.SetActive(false);
                StartCoroutine(ResetPickup());

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        pickUpPanel.SetActive(false);
    }

    private IEnumerator ResetPickup()
    {
        yield return new WaitForSeconds(0.1f);
        isPickingUp = false;
    }


    public List<Item> GetPlayerInventory()
    {
        List<Item> allItems = new List<Item>();
        allItems.AddRange(healthpotion);
        allItems.AddRange(mamaPotion);
        allItems.AddRange(weaponList);
        return allItems;
    }
}
