using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Potion : Item
{

    public int potionStrength;
    public PotionSO.PotionType type;


    private void Start()
    {
        base.Start();

        if (itemSo is PotionSO potion) // Check if itemSo is actually a PotionSO
        {
            potionStrength = potion.regenvalue;
            type = potion.potionType;
        }
    }



}
