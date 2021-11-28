using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : Interactable
{
    PlayerManager playerManager;
    CharaterStats myStats;
    EquipmentManager equipment;
    Transform target;
    PlayerStat playerStatus;

    NotifySystem notify;

    InventoryUI ui;

    private void Start()
    {
        equipment = EquipmentManager.instance;
        playerManager = PlayerManager.instance;
        notify = NotifySystem.instance;
        myStats = GetComponent<CharaterStats>();
        target = PlayerManager.instance.player.transform;
        playerStatus = playerManager.player.GetComponent<PlayerStat>();
        ui = GameObject.FindWithTag("MainUI").GetComponent<InventoryUI>();
    }


    void LateUpdate()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= playerStatus.lookRadius)
        {
            if (Input.GetButtonDown("Fire1") && ui.UINumber <= 0)
            {
                CharacterCombat playerCombat = playerManager.player.GetComponent<CharacterCombat>();
                if (playerCombat != null)
                {                   
                    var temp = equipment.currentEquipment;
                    for (int i = 0; i < temp.Length; i++)
                    {
                        //check whether or not player equiped
                        //temp[5] is the plant where containt weapon
                        if (temp[5] != null)
                        {
                            if (temp[5].item != null && temp[5].item.equipmentType == EquipmentType.Hoe)
                            {
                                playerCombat.Attack(myStats);
                                Debug.Log("Player Colleting Rock.");
                            }
                            else
                            {
                                notify.SimpleNotify("You need a hoe for working.");
                                Debug.Log("You need a hoe for working.");
                                break;
                            }
                        }
                        else
                        {
                            notify.SimpleNotify("You need a hoe for working.");
                            Debug.Log("You need a hoe for working.");
                            break;
                        }
                    }
                }
            }
        }
    }



    public override void Interact()
    {
        base.Interact();

    }
}
