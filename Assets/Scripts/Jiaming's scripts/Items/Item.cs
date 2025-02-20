using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] protected ItemSO itemSo;


    public string itemName;
    public string discription;
    protected bool isStackable;
    public Sprite uiImage;
    public int price;

    protected void Start()
    {
        itemName = itemSo.itemName;
        discription = itemSo.description;
        isStackable = itemSo.stackable;
        uiImage = itemSo.UI_Image;
        price = itemSo.price;
    }
    public void PickUp(GameObject Caller)
    {
        gameObject.SetActive(false);
        gameObject.transform.parent = Caller.transform;
        gameObject.transform.localPosition = Vector3.zero;
    }
    //can be displayed in shop,
    //can be picked up
    //
}
