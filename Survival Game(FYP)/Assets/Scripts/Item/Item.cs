using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName ="Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item"; // Name of the item
    public Sprite icon = null;           // Item icon
    public bool isDefaultItem = false;   // Is theitrm default wear?
    public bool isMany;
    public GameObject itemGameObject;

    public float decreaseRate;

    public virtual void Use()
    {
        // Use the item
        // Something might happen

        Debug.Log("Using " + name);
        
    }

    public void RemoveFromInventory()
    {
        Inventory.instance.Remove(this);
    }
}

