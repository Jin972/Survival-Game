using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockStatus : CharaterStats
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

    public void DropItem()
    {
        for (int i = 0; i < item.Length; i++)
        {
            GameObject droppedItem = (GameObject)Instantiate(item[i], transform.position, Quaternion.identity);
        }
    }

    public override void Die()
    {
        base.Die();

        int totalRockCollect = 0;

        Destroy(gameObject);
        for (int i = 0; i < items.Length; i++)
        {
            if (inventory.items.Count > inventory.space)
            {
                notify.SimpleNotify("Your inventoty capacity not enough.");
                Instantiate(item[i], transform.position, Quaternion.identity);
            }
            else
            {
                inventory.Add(items[i]);
                totalRockCollect += 1;
            }
        }
        if (totalRockCollect > 0)
        {
            notify.SimpleNotify("+" + totalRockCollect + " Rock");
        }
    }
}
