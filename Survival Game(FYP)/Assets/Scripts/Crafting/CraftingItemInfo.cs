using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CraftingItemInfo : MonoBehaviour
{
    public RectTransform ItemRequirementGroup;
    ItemRequirementSlot[] slots;
    

    public Text craftingItemName;
    public Text craftingItemDetail;
    public Image craftingItemIcon;

    CraftingItemList craftingItemList;

    internal int itemID;
    internal Item item;

    Inventory inventory;

    List<MaterialsRequest> materials;

    private void Start()
    {
        inventory = Inventory.instance;
        craftingItemList = CraftingItemList.instance;
        slots = ItemRequirementGroup.GetComponentsInChildren<ItemRequirementSlot>();
        Default();
    }

    public void ShowItemRequirement(int itemID)
    {
        this.itemID = itemID;
        craftingItemName.text = "" + craftingItemList.itemCraftingList[itemID].item.name;
        craftingItemDetail.text = "None";
        craftingItemIcon.enabled = true;
        craftingItemIcon.sprite = craftingItemList.itemCraftingList[itemID].item.icon;
        materials = craftingItemList.itemCraftingList[itemID].materialsRequests;
        for (int i = 0; i < slots.Length; i++)
        {
            if(i < materials.Count)
            {
                slots[i].AddItem(materials[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }          
        }
    }
    public void DraftButton()
    {
        List<MaterialsRequest> materials = craftingItemList.itemCraftingList[itemID].materialsRequests;
        //check player' inventory whether or not satisfy the conditions to craft item
        if (checkItemList(materials, inventory.items) == true)
        {
            Inventory.instance.Add(item);
            //clear items that use to craft new item
            ClearItems(materials);
            ShowItemRequirement(itemID);
        }
    }

    static int CountOccurenceOfValue(List<ListItem> list, Item valueToFind)
    {
        return ((from temp in list where temp.item.Equals(valueToFind) select temp).Count());
    }

    public bool checkItemList(List<MaterialsRequest> listCompare, List<ListItem> listWasComapred)
    {
        for (int i = 0; i < listCompare.Count; i++)
        {
            if (listCompare[i].Amount > CountOccurenceOfValue(listWasComapred, listCompare[i].item))
            {
                return false;
            }
        }
        return true;
    }

    void ClearItems(List<MaterialsRequest> listCompare)
    {
        for (int i = 0; i < listCompare.Count; i++)
        {           
            for (int y = 0; y < listCompare[i].Amount; y++)
            {             
                inventory.Remove(listCompare[i].item);
            }
        }
    }

    public void Default()
    {
        item = craftingItemList.itemCraftingList[0].item;
        ShowItemRequirement(0);
        itemID = 0;
    }
}
