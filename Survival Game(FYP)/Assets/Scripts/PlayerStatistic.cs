using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New File Save", menuName = "Save/File")]
public class PlayerStatistic : ScriptableObject, ISerializationCallbackReceiver
{
    public List<Item> inventory;
    public Dictionary<Item, int> GetItemID = new Dictionary<Item, int>();
    public Dictionary<int, Item> GetItem = new Dictionary<int, Item>();

    public Equipment[] currentEquipment;
    public Dictionary<Equipment, int> GetEquipemtID = new Dictionary<Equipment, int>();
    public Dictionary<int, Equipment> GetEquipemt = new Dictionary<int,Equipment>();


    public List<GameObject> currentHouse;
    public Dictionary<GameObject, int> GetHouseItemID = new Dictionary<GameObject, int>();
    public Dictionary<int, GameObject> GetHouseItem = new Dictionary<int, GameObject>();

    public void OnAfterDeserialize()
    {
        GetItemID = new Dictionary<Item, int>();
        GetItem = new Dictionary<int, Item>();    
        for(int i = 0; i < inventory.Count; i++)
        {
            GetItemID.Add(inventory[i], i);
            GetItem.Add(i, inventory[i]);
        }

        GetEquipemtID = new Dictionary<Equipment, int>();
        GetEquipemt = new Dictionary<int, Equipment>();
        for (int i = 0; i < currentEquipment.Length; i++)
        {
            GetEquipemtID.Add(currentEquipment[i], i);
            GetEquipemt.Add(i, currentEquipment[i]);
        }

        GetHouseItemID = new Dictionary<GameObject, int>();
        GetHouseItem = new Dictionary<int, GameObject>();
        for (int i = 0; i < currentHouse.Count; i++)
        {
            GetHouseItemID.Add(currentHouse[i], i);
            GetHouseItem.Add(i, currentHouse[i]);
        }
    }

    public void OnBeforeSerialize()
    {
        
    }
}


