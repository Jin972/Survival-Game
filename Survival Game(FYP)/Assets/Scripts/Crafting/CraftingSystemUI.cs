using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystemUI : MonoBehaviour
{
    public GameObject CrafingSystemUI;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Crafting"))
        {
            CrafingSystemUI.SetActive(!CrafingSystemUI.activeSelf);
        }
    }
}
