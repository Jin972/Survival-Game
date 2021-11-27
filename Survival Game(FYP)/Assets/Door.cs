using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    Transform doorMain;

    public float smooth = 3f;

    private Quaternion DoorOpen;
    private Quaternion DoorClosed;

    Animator anim;

    void Start()
    {
        anim = doorMain.gameObject.GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player") {
            anim.SetTrigger("isOpen");

            Debug.Log("Door Opened");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            anim.SetTrigger("isClose");

            Debug.Log("Door Closed");
        }
    }
}
