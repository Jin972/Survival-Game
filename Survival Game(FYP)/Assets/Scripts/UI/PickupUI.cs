using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupUI : MonoBehaviour
{
    //Set gameObject
    public GameObject uiPrefab;
    public Transform targets;

    internal Transform ui;
    Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;

        foreach (Canvas c in FindObjectsOfType<Canvas>())
        {
            if (c.renderMode == RenderMode.WorldSpace)
            {
                ui = Instantiate(uiPrefab, c.transform).transform;
                ui.gameObject.SetActive(false);
                break;
            }
        }
    }

    public void ShowAnimator()
    {
        if (ui != null)
        {
            ui.gameObject.SetActive(true);
            ui.position = targets.position;
            ui.forward = -cam.forward;
        }
    }

    public void DisableAnimator()
    {
        if (ui != null)
        {
            ui.gameObject.SetActive(false);
        }
    }

}
