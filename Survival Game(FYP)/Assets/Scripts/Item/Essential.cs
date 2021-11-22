using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Essential")]
public class Essential : Item
{

    public EssentialType essentialType;

    public override void Use()
    {
        base.Use();
        EssentialManager.instance.UserEssential(this);
        
        RemoveFromInventory();

    }
}

public enum EssentialType { Water, Food, Heath }
