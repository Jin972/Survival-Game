using StarterAssets;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    //Crafting
    public GameObject CraftingUI;

    //Setting
    public GameObject SettingUI;

    //Guide
    public GameObject guideUI;

    Inventory inventory;
    internal Treasure treasure;

    public int UINumber = 0;

    public StarterAssetsInputs playerInput;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        treasueSlots = TreasureSlotGroup.GetComponentsInChildren<TreasureSlot>();

        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            Guild();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            TriggerInventory();
        }

        if (Input.GetButtonDown("Crafting")) {
            TriggerCrafting();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            TriggerSetting();
        }

        if (UINumber > 0)
        {
            Cursor.lockState = CursorLockMode.None;
            playerInput.cursorInputForLook = false;
        }
        else {
            Cursor.lockState = CursorLockMode.Locked;
            playerInput.cursorInputForLook = true;
        }

        if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void Guild()
    {
        guideUI.SetActive(!guideUI.activeSelf);
        if (guideUI.activeSelf == true)
        {
            UINumber += 1;
        }
        else
        {
            UINumber -= 1;
            if(UINumber < 0)
            {
                UINumber = 0;
            }
        }
    }

    public void TriggerSetting()
    {
        SettingUI.SetActive(!SettingUI.activeSelf);
        if (SettingUI.activeSelf == true)
        {
            UINumber += 1;
        }
        else
        {
            UINumber -= 1;
        }
    }

    public void TriggerInventory() {
        UpdateUI();
        background.SetActive(!background.activeSelf);
        inventoryUI.SetActive(!inventoryUI.activeSelf);
        displayUI.SetActive(!displayUI.activeSelf);
        CloseButton.SetActive(!CloseButton.activeSelf);
        if (inventoryUI.activeSelf == true)
        {
            UINumber += 1;
        }
        else
        {
            UINumber -= 1;
        }
    }

    public void TriggerCrafting()
    {
        CraftingUI.SetActive(!CraftingUI.activeSelf);
        if (CraftingUI.activeSelf == true)
        {
            UINumber += 1;
        }
        else
        {
            UINumber -= 1;
        }
    }

    public void TurnOnTreasure()
    {
        if(TreasureUI.activeSelf == false)
        {
            background.SetActive(true);
            inventoryUI.SetActive(true);
            TreasureUI.SetActive(true);
            CloseButton.SetActive(true);
            UINumber += 1;
        }
    }

    public void TurnOffTreasure()
    {
            background.SetActive(false);
            inventoryUI.SetActive(false);
            TreasureUI.SetActive(false);
            CloseButton.SetActive(false);
            UINumber -= 1;  
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

