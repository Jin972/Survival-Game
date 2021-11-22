using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharaterStats))]
public class Enemy : Interactable
{
    PlayerManager playerManager;
    CharaterStats myStats;
    PlayerStat player;
    PlayerMotor motor;
    Transform target;

    private void Start()
    {
        playerManager = PlayerManager.instance;
        myStats = GetComponent<CharaterStats>();
        motor = playerManager.player.GetComponent<PlayerMotor>();
        target = playerManager.player.transform;
        player = playerManager.player.GetComponent<PlayerStat>();
    }

    private void LateUpdate()
    {
        //Attack with agent
        float distance = Vector3.Distance(target.position, transform.position);

        //if (distance <= player.lookRadius)
        //    {
        //        print("can attack");
        //        if (Input.GetMouseButton(0))
        //        {
        //            motor.agent.stoppingDistance = player.attackRange.GetValue();
        //            motor.MoveToPoint(transform.position);

        //            if (distance <= motor.agent.stoppingDistance)
        //            {
        //                CharacterCombat playerCombat = playerManager.player.GetComponent<CharacterCombat>();
        //                if (playerCombat != null)
        //                {
        //                    playerCombat.Attack(myStats);

        //                }
        //                FaceTarget();
        //            }
        //        }
        //    }

        if (distance <= player.lookRadius)
        {
            print("can attack");
            if (Input.GetMouseButton(0))
            {            
                if (distance <= player.attackRange.GetValue())
                {
                    FaceTarget();
                    CharacterCombat playerCombat = playerManager.player.GetComponent<CharacterCombat>();
                    if (playerCombat != null)
                    {
                        playerCombat.Attack(myStats);
                        print("Player attack");
                    }                 
                }
            }
        }
    }

    public override void Interact()
    {
        base.Interact();
        
    }

    void FaceTarget()
    {
        Vector3 direction = (transform.position - target.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        target.rotation = Quaternion.Slerp(target.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
