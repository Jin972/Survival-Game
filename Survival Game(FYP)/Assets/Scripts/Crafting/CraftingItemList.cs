using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CraftingItemList : MonoBehaviour
{
    #region Singleton
    public static CraftingItemList instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than iten that found");
            return;
        }
        instance = this;
    }
    #endregion

    public RectTransform CraftingItemParent;
    public CraftingItemSlot slots;
    public CraftingItemInfo craftingItemInfo;

    [System.Serializable]
    public class ItemCrafting
    {
        [HideInInspector]
        public int itemID;
        public Item item;
        public List<MaterialsRequest>  materialsRequests;

        [HideInInspector]
        public CraftingItemSlot slot;
    }

    public ItemCrafting[] itemCraftingList;

    // Start is called before the first frame update
    void Start()
    {
        InitializeSlot();
        UpdateItems();
    }

    void InitializeSlot()
    {
        for (int i = 0; i < itemCraftingList.Length; i++)
        {
            if (itemCraftingList[i] == null)
            {
                itemCraftingList[i] = new ItemCrafting();
            }
            GameObject newSlot = Instantiate(slots.gameObject, CraftingItemParent.transform);
            itemCraftingList[i].slot = newSlot.GetComponent<CraftingItemSlot>();
            itemCraftingList[i].slot.gameObject.SetActive(true);        
        }
    }

    void UpdateItems()
    {
        for (int i = 0; i < itemCraftingList.Length; i++)
        {
            itemCraftingList[i].slot.itemID = i;
            itemCraftingList[i].slot.craftingItemInfo = craftingItemInfo;

            itemCraftingList[i].slot.item = itemCraftingList[i].item;
            //Apply item icon
            itemCraftingList[i].slot.icon.enabled = true;
            itemCraftingList[i].slot.icon.sprite = itemCraftingList[i].item.icon;
            itemCraftingList[i].slot.itemName.enabled = true;
            itemCraftingList[i].slot.itemName.text = itemCraftingList[i].item.name; ;
        }
    }

}

[System.Serializable]
public class MaterialsRequest
{
    public Item item;
    public int Amount;
}