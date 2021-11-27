using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    NotifySystem notify;

    private void Start()
    {
        notify = NotifySystem.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            notify.ConfirmDialogChangeMap();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            print("Player exit");
            notify.Cancel();
        }
    }
}
