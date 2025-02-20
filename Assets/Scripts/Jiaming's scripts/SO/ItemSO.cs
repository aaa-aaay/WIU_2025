using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class ItemSO : ScriptableObject
{
    public string itemName;
    public string description;
    public bool stackable;
    public Sprite UI_Image;
    public int price = 10;

}
