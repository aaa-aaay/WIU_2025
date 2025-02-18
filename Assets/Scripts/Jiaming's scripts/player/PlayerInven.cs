using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInven : MonoBehaviour
{
    // can put this in a child object of the player
    private KeyCode tempKeyCode = KeyCode.E;
    private KeyCode tempKeyCodeSwitchPotion = KeyCode.DownArrow;
    private KeyCode tempKeyCodeSwitchPotion2 = KeyCode.UpArrow;
    [SerializeField] private SphereCollider pickupRange;

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
    }

    private void Update()
    {
        if(Input.GetKeyDown(tempKeyCodeSwitchPotion) || Input.GetKeyDown(tempKeyCodeSwitchPotion2))
        {
            if (currentPotionDisplayed == 1)
                currentPotionDisplayed = 2;
            else if (currentPotionDisplayed == 2) 
                currentPotionDisplayed = 1;

            UpdatePotionUI();
        }
    }


    public void UpdatePotionUI()
    {
        if (currentPotionDisplayed == 1)
        {
            OnInventoryUpdated?.Invoke(healthpotion.First().uiImage, healthpotion.Count);
            //update the ui with the new count;
        }
        else if (currentPotionDisplayed == 2)
        {
            OnInventoryUpdated?.Invoke(mamaPotion.First().uiImage, mamaPotion.Count);
        }


        else if (currentPotionDisplayed == 0)
        {
            if (healthpotion.Count > 0) currentPotionDisplayed = 1;
            else if (mamaPotion.Count > 0) currentPotionDisplayed = 2;

            UpdatePotionUI();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("hit smth");
        Item item = other.GetComponent<Item>();
        if(item != null)
        {
            //TODO: Show the Pick UI
            if (Input.GetKeyDown(tempKeyCode) && !isPickingUp  )
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
                StartCoroutine(ResetPickup());

            }
        }
    }

    private IEnumerator ResetPickup()
    {
        yield return new WaitForSeconds(0.1f);
        isPickingUp = false;
    }
}
