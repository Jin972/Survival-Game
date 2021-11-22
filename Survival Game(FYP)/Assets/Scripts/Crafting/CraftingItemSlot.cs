using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingItemSlot : MonoBehaviour
{
    internal int itemID;
    public Image icon;
    internal Item item;
    public Text itemName;

    public CraftingItemInfo craftingItemInfo;

    private void Start()
    {
    }

    public void ShowItemInfo()
    {    
        craftingItemInfo.ShowItemRequirement(itemID);
        craftingItemInfo.item = item;
        craftingItemInfo.itemID = itemID;
    }
}
