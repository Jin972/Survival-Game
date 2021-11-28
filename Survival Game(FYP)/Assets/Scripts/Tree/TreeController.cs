using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : CharaterStats
{
    public GameObject[] item;

    [SerializeField]
    Item[] items;

    Inventory inventory;
    NotifySystem notify;

    private void Start()
    {
        inventory = Inventory.instance;
        notify = NotifySystem.instance;
    }

    public override void Die()
    {
        base.Die();

        int totalWoodCollect = 0;

        Destroy(gameObject);
        for(int i = 0; i < items.Length; i++)
        {
            if(inventory.items.Count > inventory.space)
            {
                notify.SimpleNotify("Your inventoty capacity not enough.");
                Instantiate(item[i], transform.position, Quaternion.identity);
            }
            else
            {
                inventory.Add(items[i]);
                totalWoodCollect += 1;  
            }   
        }

        if(totalWoodCollect > 0)
        {
            notify.SimpleNotify("+" + totalWoodCollect + " Wood");
        }
    }
}
