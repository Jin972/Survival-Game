using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonAnimator : MonoBehaviour
{
    public AnimationClip replaceableAttackAnin;
    public AnimationClip[] defaultAttackAnimSet;
    protected AnimationClip[] currentAttackAnimSet;

    protected Animator animator;
    protected CharacterCombat combat;

    public AnimatorOverrideController overrideController;

    public WeaponAnimation[] weaponAnimations;
    Dictionary<Equipment, AnimationClip[]> weaponAnimationDict;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        combat = GetComponent<CharacterCombat>();

        if (overrideController == null)
        {
            overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        }
        animator.runtimeAnimatorController = overrideController;

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
        int attackIndex = Random.Range(0, currentAttackAnimSet.Length);
        overrideController[replaceableAttackAnin.name] = currentAttackAnimSet[attackIndex];
    }

    void OnEquipmentChanged(Equipment newItem, Equipment oldIten)
    {
        if (newItem != null && newItem.equipSlot == EquipmentSlot.Weapon)
        {
            //animator.SetLayerWeight(1, 1);
            if (weaponAnimationDict.ContainsKey(newItem))
            {
                currentAttackAnimSet = weaponAnimationDict[newItem];
            }
        }
        else if (newItem == null && oldIten.equipSlot == EquipmentSlot.Weapon)
        {
            //animator.SetLayerWeight(1, 0);
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
