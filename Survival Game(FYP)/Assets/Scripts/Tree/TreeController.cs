using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : CharaterStats
{
    public GameObject[] item;

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
    }
  
}
