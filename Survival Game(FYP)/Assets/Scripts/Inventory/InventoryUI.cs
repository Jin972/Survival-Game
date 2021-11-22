using StarterAssets;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    //Inventory
    public Transform itemsParent;
    public GameObject background;
    public GameObject inventoryUI;
    public GameObject displayUI;
    InventorySlot[] slots;

    //Treasure
    public RectTransform TreasureSlotGroup;
    TreasureSlot[] treasueSlots;
    public GameObject CloseButton;
    public GameObject TreasureUI;

    Inventory inventory;
    internal Treasure treasure;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        treasueSlots = TreasureSlotGroup.GetComponentsInChildren<TreasureSlot>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            UpdateUI();
            background.SetActive(!background.activeSelf);
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            displayUI.SetActive(!displayUI.activeSelf);
            CloseButton.SetActive(!CloseButton.activeSelf);        
        }
    }

    public void TurnOnTreasure()
    {
        background.SetActive(true);
        inventoryUI.SetActive(true);
        TreasureUI.SetActive(true);
        CloseButton.SetActive(true);
    }

    public void TurnOffTreasure()
    {
        background.SetActive(false);
        inventoryUI.SetActive(false);
        TreasureUI.SetActive(false);
        CloseButton.SetActive(false);
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i].item);      
            }
            else
            {
                slots[i].ClearSlot();
            }
        }        
    }

    public void ShowTreasureItem(List<Item> items)
    {
        for (int i = 0; i < treasueSlots.Length; i++)
        {
            if (i < items.Count)
            {
                treasueSlots[i].AddItem(items[i], this, i);
            }
            else
            {
                treasueSlots[i].ClearSlot();
            }

        }
    }

    public void CloseTreasure()
    {
        TurnOffTreasure();
        if(displayUI.activeSelf == true)
        {
            displayUI.SetActive(false);
        }
        for (int i = 0; i < treasueSlots.Length; i++)
        {
            treasueSlots[i].ClearSlot();
        }
    }

    public void TakeAllItem()
    {
        for (int i = 0; i < treasueSlots.Length; i++)
        {
            if(treasueSlots[i].item != null)
            {
                inventory.Add(treasueSlots[i].item);
                treasueSlots[i].ClearSlot();
                
                UpdateUI();
            }
        }
        treasure.items.Clear();
    }

    public void ClearItemInTreasure(int itemID)
    {
        treasure.items.RemoveAt(itemID);
    }
}

