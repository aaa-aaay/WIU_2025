using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DescPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text nameTxt;
    [SerializeField] private TMP_Text descTxt;
    [SerializeField] private TMP_Text priceTxt;

    public void ChangeDescPanelInfo(string name, string desc, int price) {
        nameTxt.text = name;
        descTxt.text = desc;
        priceTxt.text = price.ToString();
    }

}
