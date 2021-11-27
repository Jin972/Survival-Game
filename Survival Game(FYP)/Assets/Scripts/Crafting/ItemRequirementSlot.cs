using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemRequirementSlot : MonoBehaviour
{
    public Image icon;

    Item item;

    public Text Amount;

    Inventory inventory;

    public delegate void SlotChangeCallBack();
    public SlotChangeCallBack slotChangeCallBack;
  
    private void Start()
    {
        inventory = Inventory.instance;
    }

    public void AddItem(MaterialsRequest newItem)
    {
        item = newItem.item;

        if (inventory != null)
        {
            Amount.text = CountOccurenceOfValue(inventory.items, item) + "/" + newItem.Amount;
        }
        else {
            Amount.text = "0/" + newItem.Amount;
        }
              
        

        icon.sprite = newItem.item.icon;
        icon.enabled = true;
        Amount.enabled = true;
        if (slotChangeCallBack != null)
            slotChangeCallBack.Invoke();
    }

    public void ClearSlot()
    {
        item = null;

        Amount.text = null;
        icon.sprite = null;
        icon.enabled = false;
        Amount.enabled = false;
        if (slotChangeCallBack != null)
            slotChangeCallBack.Invoke();
    }

    static int CountOccurenceOfValue(List<ListItem> list, Item valueToFind)
    {
        return ((from temp in list where temp.item.Equals(valueToFind) select temp).Count());
    }
}
