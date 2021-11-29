using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMenu : MonoBehaviour
{
    [SerializeField]
    MenuUI menu;

    InventoryUI ui;

    [SerializeField]
    GameObject configureContent;
    [SerializeField]
    GameObject guideContent;
    [SerializeField]
    GameObject aboutContent;

    public int part = 1;

    // Start is called before the first frame update
    void Start()
    {
        ui = GameObject.FindWithTag("MainUI").GetComponent<InventoryUI>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (part)
        {
            case 1:
                configureContent.SetActive(true);
                guideContent.SetActive(false);
                aboutContent.SetActive(false);
                break;
            case 2:
                configureContent.SetActive(false);
                guideContent.SetActive(true);
                aboutContent.SetActive(false);
                break;
            case 3:
                configureContent.SetActive(false);
                guideContent.SetActive(false);
                aboutContent.SetActive(true);
                break;
        }
    }

    public void ConfigureButton()
    {
        part = 1;
    }

    public void GuideButton()
    {
        part = 2;
    }

    public void AboutButton()
    {
        part = 3;
    }

    public void ExitButton()
    {
        if(menu != null)
        {
            menu.Pause();
            gameObject.SetActive(!gameObject.activeSelf);
            ui.UINumber -= 1;
            if(ui.UINumber < 0)
            {
                ui.UINumber = 0;
            }
        }
    }
}
