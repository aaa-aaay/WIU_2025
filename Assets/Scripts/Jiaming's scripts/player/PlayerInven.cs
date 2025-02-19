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
            if(healthpotion.Count > 0 || mamaPotion.Count > 0)
            {
                if (healthpotion.Count > 0) currentPotionDisplayed = 1;
                else if (mamaPotion.Count > 0) currentPotionDisplayed = 2;
                UpdatePotionUI();
            }

             

        }
    }

    private void UsePotion()
    {
        if (currentPotionDisplayed == 1)
        {
            healthpotion.Remove(healthpotion.Last());
            //increase the player health (set it either here or in the item script)
            playerStats.Heal();
            if (healthpotion.Count == 0) currentPotionDisplayed = 2;
            UpdatePotionUI();


            //use health potion
        }
        else if (currentPotionDisplayed == 2)
        {
            mamaPotion.Remove(mamaPotion.Last());
            //increase the player mana (set it either here or in the item script)
            if (mamaPotion.Count == 0) currentPotionDisplayed = 1;
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


                item.PickUp();
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
}
