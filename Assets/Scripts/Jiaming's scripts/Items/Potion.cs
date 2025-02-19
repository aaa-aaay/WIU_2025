using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Potion : Item
{

    private int potionStrength;
    public PotionSO.PotionType type;


    private void Start()
    {
        if (itemSo is PotionSO potion) // Check if itemSo is actually a PotionSO
        {
            potionStrength = potion.regenvalue;
            type = potion.potionType;
        }
    }
    public override void PickUp() // maybe play a different sfx here or smth
    {
        Destroy(gameObject);
    }



}
