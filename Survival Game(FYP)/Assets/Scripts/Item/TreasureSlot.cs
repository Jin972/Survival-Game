using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureSlot : MonoBehaviour
{
    public Image icon;

    internal Item item;

    Inventory inventory;

    InventoryUI ui;

    int itemID;

    private void Start()
    {
        inventory = Inventory.instance;
    }

    public void AddItem(Item newItem, InventoryUI ui, int index)
    {
        this.ui = ui;
        item = newItem;
        itemID = index;

        icon.sprite = newItem.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
    }

    public void AddItemToInventory()
    {
        inventory.Add(item);
        ui.ClearItemInTreasure(itemID);
        ClearSlot();
    }
}
