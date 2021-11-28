using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;
    [SerializeField]
    private float pikupRadius = 3f;
    Transform target;
    PickupUI ui;

    Inventory inventory;
    NotifySystem notify;

    private void Start()
    {
        inventory = Inventory.instance;
        notify = NotifySystem.instance;
        target = PlayerManager.instance.player.transform;
        ui = GetComponent<PickupUI>();
    }

   
    void LateUpdate()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if(distance <= pikupRadius)
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
    }

    public virtual void PickUp()
    {
        Debug.Log("Picking up item " + item.name);
        //Add to inventory
        if(inventory.items.Count < inventory.space)
        {
            inventory.Add(item);
            ui.DisableAnimator();
            Destroy(gameObject);
        }
        else
        {
            notify.SimpleNotify("Your inventoty capacity not enough.");
        }
        
    }

}
