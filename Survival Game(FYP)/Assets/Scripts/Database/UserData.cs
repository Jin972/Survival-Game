using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData 
{
    public int health;
    public float thirst;
    public float hunger;
    public float[] position;

    public UserData(int health, float thirst, float hunger, float[] position)
    {
        this.health = health;
        this.thirst = thirst;
        this.hunger = hunger;
        this.position = position;
    }
}

public class InventoryData
{
    public List<int> inventorys;

    public InventoryData(List<int> inventorys)
    {
        this.inventorys = inventorys;
    }
}

public class EquipementData
{
    public List<equipment> equipments;

    public EquipementData(List<equipment> equipments)
    {
        this.equipments = equipments;
    }
}

public class buildingData
{
    public List<building> buildings;

    public buildingData(List<building> buildings)
    {
        this.buildings = buildings;
    }
}

[System.Serializable]
public class equipment
{
    public int equipID;
    public string name;
    public equipment(int equipID, string name)
    {
        this.name = name;
        this.equipID = equipID;
    }
}

[System.Serializable]
public class building
{
    public int id;
    public float[] position;
    public float[] rotation;
    public building(int id, float[] position, float[] rotation)
    {
        this.id = id;
        this.position = position;
        this.rotation = rotation;
    }
}
