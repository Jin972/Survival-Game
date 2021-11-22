using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    private bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    PlayerManager manager;
    PlayerStat status;
    BuildingSystem building;
    EquipmentManager equip;
    Inventory inventory;
    LoadSence load;

    private void Start()
    {
        manager = PlayerManager.instance;
        status = manager.player.GetComponent<PlayerStat>();
        building = BuildingSystem.instance;
        equip = EquipmentManager.instance;
        inventory = Inventory.instance;
        load = LoadSence.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        GameIsPaused = false;
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Debug.Log("Reset...");
        
        status.DeleteFile();
        building.DeleteFile();
        equip.DeleteFile();
        inventory.DeleteFile();
        load.LoadLevel(0);
        Debug.Log("Reseted");
    }

    public void Quit()
    {
        Debug.Log("Quit Game");
        status.Save();
        //building.Save();
        //equip.Save();
        //inventory.Save();
        Application.Quit();
    }
}
