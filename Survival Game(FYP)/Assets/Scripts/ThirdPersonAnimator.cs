using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonAnimator : MonoBehaviour
{
    public AnimationClip[] defaultAttackAnimSet;
    protected AnimationClip[] currentAttackAnimSet;

    protected Animator animator;
    protected CharacterCombat combat;

    public WeaponAnimation[] weaponAnimations;
    Dictionary<Equipment, AnimationClip[]> weaponAnimationDict;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        combat = GetComponent<CharacterCombat>();

        currentAttackAnimSet = defaultAttackAnimSet;
        combat.OnAttack += OnAttack;

        EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;

        weaponAnimationDict = new Dictionary<Equipment, AnimationClip[]>();
        foreach (WeaponAnimation a in weaponAnimations)
        {
            weaponAnimationDict.Add(a.weapon, a.clips);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //animator.SetBool("isCombat", combat.IsCombat);
    }

    protected virtual void OnAttack()
    {
        animator.SetTrigger("isAttack");
    }

    void OnEquipmentChanged(Equipment newItem, Equipment oldIten)
    {
        if (newItem != null && newItem.equipSlot == EquipmentSlot.Weapon)
        {
            animator.SetLayerWeight(1, 1);
            if (weaponAnimationDict.ContainsKey(newItem))
            {
                currentAttackAnimSet = weaponAnimationDict[newItem];
            }
        }
        else if (newItem == null && oldIten.equipSlot == EquipmentSlot.Weapon)
        {
            animator.SetLayerWeight(1, 0);
            currentAttackAnimSet = defaultAttackAnimSet;
        }


    }

    [System.Serializable]
    public struct WeaponAnimation
    {
        public Equipment weapon;
        public AnimationClip[] clips;
    }
}
