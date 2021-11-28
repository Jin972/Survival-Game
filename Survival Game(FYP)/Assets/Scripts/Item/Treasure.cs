using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : Interactable
{
    public InventoryUI inventoryUI;

    public List<Item> items;
    [SerializeField]
    private float pikupRadius = 3f;
    Transform target;
    PickupUI ui;

    [SerializeField]
    PlayerStatistic database;

    [SerializeField]
    bool isBig;

    private void Start()
    {
        target = PlayerManager.instance.player.transform;
        ui = GetComponent<PickupUI>();
        if(inventoryUI == null)
        {
            inventoryUI = GameObject.FindWithTag("MainUI").GetComponent<InventoryUI>();
        }

        int itemsAmount;
        if (isBig)
        {
            itemsAmount = Random.Range(5, 7);

        }
        else
        {
            itemsAmount = Random.Range(3, 5);
        }

        for (int i = 0; i < itemsAmount; i++)
        {
            int itemIndex = Random.Range(0, database.inventory.Count);
            items.Add(database.inventory[itemIndex]);
        }
    }

    void LateUpdate()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= pikupRadius)
        {
            ui.ShowAnimator();
            if (Input.GetKeyDown(KeyCode.F))
            {
                PickUp();
            }
        }
        else
        {
            ui.DisableAnimator();
        }

        if (items.Count <= 0)
        {
            StartCoroutine(WaitToDisappear(60f)); 
        }

    }

    IEnumerator WaitToDisappear(float second)
    {
        yield return new WaitForSeconds(second);
        Destroy(gameObject);
    }

    public override void Interact()
    {
        base.Interact();
        PickUp();
    }

    public void PickUp()
    {
        inventoryUI.ShowTreasureItem(items);
        inventoryUI.treasure = this;
        inventoryUI.TurnOnTreasure();
        ui.DisableAnimator();
    }
}