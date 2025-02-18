using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInven : MonoBehaviour
{
    

    // can put this in a child object of the player
    private KeyCode tempKeyCode = KeyCode.E;
    [SerializeField] private SphereCollider pickupRange;
    [SerializeField]
    private List<Potion> healthpotion = new List<Potion>();
    private List<Potion> mamaPotion = new List<Potion>();
    private List<Weapon> weaponList = new List<Weapon>();

    private void Start()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("hit smth");
        Item item = other.GetComponent<Item>();
        if(item != null)
        {
            //TODO: Show the Pick UI
            if (Input.GetKeyDown(tempKeyCode))
            {


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
                }



                else if(item is Weapon)
                {
                    weaponList.Add((Weapon)item);
                }


                item.PickUp();




            }
        }
    }
}
