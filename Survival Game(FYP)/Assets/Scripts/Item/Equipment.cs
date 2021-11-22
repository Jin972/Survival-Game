using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName ="Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentSlot equipSlot;
    public EquipmentType equipmentType;
    public SkinnedMeshRenderer mesh;
    public SkinnedMeshRenderer meshGameObject;
    public GameObject weapon;
    public EquipmentMeshRegion[] covereMeshRegion;

    public int armorModifier;
    public int damageModifier;
    public int attckRangeModifier;


    public override void Use()
    {

        EquipmentManager.instance.Equip(this);
       
        RemoveFromInventory();
    }
}

public enum EquipmentSlot {  FullHead, HalfHead, Chest, ChestOutSide, Legs, Weapon, Shield, Feet, FullBody}
public enum EquipmentType { Other, Axe, Hoe, Gun, Knife, Sword}
public enum EquipmentMeshRegion { Legs, Arms, Torso };