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

    private void Start()
    {
        target = PlayerManager.instance.player.transform;
        ui = GetComponent<PickupUI>();
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

        if (items == null)
        {
            StartCoroutine(WaitToDisappear(60f));
            Destroy(gameObject);
        }

    }

    IEnumerator WaitToDisappear(float second)
    {
        yield return new WaitForSeconds(second);
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