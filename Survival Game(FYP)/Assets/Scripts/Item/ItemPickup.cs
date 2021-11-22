using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;
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
        Inventory.instance.Add(item);
        ui.DisableAnimator();
        Destroy(gameObject);
    }

    //public override void Interact()
    //{
    //    base.Interact();
    //    PickUp();
    //}

}
