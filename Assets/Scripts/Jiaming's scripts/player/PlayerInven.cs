using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using UnityEngine;

public class PlayerInven : MonoBehaviour
{
    // can put this in a child object of the player
    private KeyCode tempKeyCode = KeyCode.E;
    private KeyCode tempKeyCode2 = KeyCode.Q;
    private KeyCode tempKeyCodeSwitchPotion = KeyCode.DownArrow;
    private KeyCode tempKeyCodeSwitchPotion2 = KeyCode.UpArrow;
    private KeyCode tempKeyCode3 = KeyCode.LeftArrow;
    private KeyCode tempKeyCode4 = KeyCode.RightArrow;
    [SerializeField] private SphereCollider pickupRange;
    //[SerializeField] private GameObject pickUpPanel;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] Transform handPosition;

    private List<Potion> healthpotion = new List<Potion>();
    private List<Potion> mamaPotion = new List<Potion>();
    private List<Weapon> weaponList = new List<Weapon>();   

    private int currentPotionDisplayed;
    private int currentWeaponDisplayed;

    public Weapon currentWeapon;
    private Potion currentPotion;

    private bool isPickingUp = false;

    public event Action<Sprite, int> OnInventoryUpdated;
    public event Action<Sprite, GameObject> OnWeaponUpdated;


    private void Start()
    {
        currentPotionDisplayed = 0;
        playerStats = gameObject.transform.parent.GetComponent<PlayerStats>();

        if(currentWeapon != null)
        {
            Debug.Log("weapon sent");
            weaponList.Add(currentWeapon);
            currentWeapon.SetWeaponPosition(handPosition);
            OnWeaponUpdated?.Invoke(currentWeapon.uiImage, currentWeapon.gameObject);
            currentWeaponDisplayed = 0;
        }

    }

    private void Update()
    {
        //switch potions
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

        //use potion
        if (Input.GetKeyDown(tempKeyCode2) && (healthpotion.Count > 0 || mamaPotion.Count > 0))
        {
            UsePotion();
        }

        //Switch Weapon
        if (Input.GetKeyDown(tempKeyCode3))
        {
            //go next weapon;
            if (currentWeaponDisplayed > 0) {

                currentWeaponDisplayed--;
                currentWeapon = weaponList[currentWeaponDisplayed];
                OnWeaponUpdated?.Invoke(currentWeapon.uiImage, currentWeapon.gameObject);
            }

                

        }
        if(Input.GetKeyDown(tempKeyCode4))
        {
            if (currentWeaponDisplayed < weaponList.Count - 1) {

                currentWeaponDisplayed++;
                currentWeapon = weaponList[currentWeaponDisplayed];
                OnWeaponUpdated?.Invoke(currentWeapon.uiImage, currentWeapon.gameObject);

            } 
            //go previous weapon;
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
            playerStats.HealPotion(usedPotion.potionStrength);

            //Remove from inven
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
            playerStats.ManaPotion(usedPotion.potionStrength);

            //Remove from inven
            mamaPotion.Remove(usedPotion);
            Destroy(usedPotion.gameObject);
  

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
           // pickUpPanel.SetActive(true);
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



                //else if(item is Weapon)
                //{
                //    weaponList.Add((Weapon)item);
                //}

                 
                item.PickUp(gameObject);
               // pickUpPanel.SetActive(false);
                StartCoroutine(ResetPickup());

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
       // pickUpPanel.SetActive(false);
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
    public void SetPlayerInventory(Item newItem, bool toAdd)
    {
        if (toAdd)
        {
            if (newItem is Potion potion)
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


            if (newItem is Weapon weapon)
            {
                weaponList.Add(weapon);
                weapon.SetWeaponPosition(handPosition);
                if(weaponList.Count == 1)
                {
                    currentWeapon = weapon;
                    OnWeaponUpdated?.Invoke(currentWeapon.uiImage, currentWeapon.gameObject);
                    currentWeaponDisplayed = 0;

                }
            }
        }


    }
}
