using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : CharaterStats
{
    public GameObject[] amountOfItem;

    public void DropItem()
    {
        for (int i = 0; i < amountOfItem.Length; i++)
        {
            GameObject droppedItem = (GameObject)Instantiate(amountOfItem[i], transform.position, Quaternion.identity);
        }
    }

    public override void Die()
    {
        base.Die();

        DropItem();
        Destroy(gameObject);
    }
}
