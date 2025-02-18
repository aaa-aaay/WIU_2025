using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] protected ItemSO itemSo;


    protected string itemName;
    protected string discription;
    protected bool isStackable;
    public Sprite uiImage;

    private void Start()
    {
        itemName = itemSo.itemName;
        discription = itemSo.description;
        isStackable = itemSo.stackable;
        uiImage = itemSo.UI_Image;
    }
    public abstract void PickUp();
    //can be displayed in shop,
    //can be picked up
    //
}
