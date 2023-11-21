using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemWidget : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI price;
    [SerializeField] TextMeshProUGUI description;
    internal void Init(ShopItem item)
    {
        icon.sprite = item.Icon;
        itemName.text = item.Title;
        price.text = "$" + item.Price.ToString();
        description.text = item.Description;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
