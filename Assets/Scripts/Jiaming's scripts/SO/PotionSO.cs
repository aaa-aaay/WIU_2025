using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "potion", menuName = "Items/potion")]
public class PotionSO : ItemSO
{
    public int regenvalue = 50;
    public PotionType potionType;

    public enum PotionType
    {
        HEALTH,
        MANA
    }
}
