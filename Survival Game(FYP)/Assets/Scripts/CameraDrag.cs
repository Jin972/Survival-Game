using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    public float speed = 20f;
    public float panBorderThickness = 10f;

    public float horizontalLimit = 10f;
    public float verticalLimit = 10f;

    [SerializeField]
    InventoryUI ui;

    void Update()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey(KeyCode.W) /*|| Input.mousePosition.y >= Screen.height - panBorderThickness*/) {
            pos.z += speed * Time.deltaTime;   
        }

        if (Input.GetKey(KeyCode.S) /*|| Input.mousePosition.y <= panBorderThickness*/)
        {
            pos.z -= speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A) /*|| Input.mousePosition.x >= Screen.height - panBorderThickness*/)
        {
            pos.x -= speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D) /*|| Input.mousePosition.x <= panBorderThickness*/)
        {
            pos.x += speed * Time.deltaTime;
        }

        pos.x = Mathf.Clamp(pos.x, -horizontalLimit, horizontalLimit);
        pos.z = Mathf.Clamp(pos.z, -verticalLimit, verticalLimit);

        transform.position = pos;
    }
}
