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

        DropItem();
        Destroy(gameObject);
        for (int i = 0; i < items.Length; i++)
        {
            inventory.Add(items[i]);
        }
        notify.SimpleNotify( "+"+ items.Length +" Rock");
    }
}
