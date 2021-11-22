using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CircleMenu : MonoBehaviour
{
    public List<MenuButton> buttons = new List<MenuButton>();
    private Vector2 MousePosition;
    private Vector2 fromVector2M = new Vector2(0.5f,1f);
    private Vector2 centerCircle = new Vector2(0.5f, 0.5f);
    private Vector2 toVector2M;

    public int menuItems;
    public int CurMenuItem;
    private int OldMenuItem;

    public BuildingSystem buildingSystem;

    Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        menuItems = buttons.Count;
        foreach(MenuButton button in buttons)
        {
            button.sceneImage.color = button.NormalColor;
        }
        CurMenuItem = 0;
        OldMenuItem = 0;
    }

    // Update is called once per frame
    void Update()
    {
        GetInfo();
        GetCurrentMenuItem();
        if (Input.GetButtonDown("Fire1"))
        {
            ButtonAction();
        }
    }

    public void GetCurrentMenuItem()
    {
        MousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        toVector2M = new Vector2(MousePosition.x / Screen.width, MousePosition.y / Screen.height );

        float angle = (Mathf.Atan2(fromVector2M.y - centerCircle.y, fromVector2M.x - centerCircle.x) - Mathf.Atan2(toVector2M.y - centerCircle.y, toVector2M.x - centerCircle.x)) * Mathf.Rad2Deg;

        if (angle < 0)
            angle += 360;

        CurMenuItem = (int)(angle / (360 / menuItems));

        if (CurMenuItem != OldMenuItem)
        {
            buttons[OldMenuItem].sceneImage.color = buttons[OldMenuItem].NormalColor;
            buttons[OldMenuItem].itemInfo.SetActive(false);
            OldMenuItem = CurMenuItem;
            buttons[CurMenuItem].sceneImage.color = buttons[OldMenuItem].HighLightedColor;
            buttons[CurMenuItem].itemInfo.SetActive(true);
        }

    }
 
    public void ButtonAction()
    {
        if(buttons[CurMenuItem] != buttons[buttons.Count - 1])
        {
            buttons[CurMenuItem].sceneImage.color = buttons[CurMenuItem].PressedColor;
            print(checkItemList(buttons[CurMenuItem].amount, buttons[CurMenuItem].itemRequest));
            if (checkItemList(buttons[CurMenuItem].amount, buttons[CurMenuItem].itemRequest) == true)
            {
                buildingSystem.isRemove = false;
                buildingSystem.ChangeCurrentBuilding(CurMenuItem);
                buildingSystem.buildingID = CurMenuItem;
                buildingSystem.TurnOffMenu();
            }
        }
        else
        {
            buildingSystem.isRemove = !buildingSystem.isRemove;
            buildingSystem.buildingID = CurMenuItem;
            buildingSystem.TurnOffMenu();
        }
        
        
    }

    

    void GetInfo()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if(buttons[i] != buttons[buttons.Count - 1])
            {
                buttons[i].imageInfo.sprite = buttons[i].itemRequest.icon;
                buttons[i].info.text = CountOccurenceOfValue(inventory.items, buttons[i].itemRequest) + "/" + buttons[i].amount;
            }
            else
            {
                buttons[i].imageInfo.sprite = null;
            }   
        }
    }

    public bool checkItemList(int amount, Item item)
    {
        if (amount <= CountOccurenceOfValue(inventory.items, item))
        {
            return true;
        }
        return false;
    }

    public void ClearItems(Item item, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            inventory.Remove(item);
        }
    }

    static int CountOccurenceOfValue(List<ListItem> list, Item valueToFind)
    {
        return ((from temp in list where temp.item.Equals(valueToFind) select temp).Count());
    }
}

[System.Serializable]
public class MenuButton {
    public string name;
    public Image sceneImage;
    public Color NormalColor = Color.white;
    public Color HighLightedColor = Color.grey;
    public Color PressedColor = Color.gray;
    public GameObject itemInfo;
    public Item itemRequest;
    public Image imageInfo;
    public Text info;
    public int amount;
}

