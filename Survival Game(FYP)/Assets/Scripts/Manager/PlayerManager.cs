using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    #region Singleton
    public static PlayerManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion
    
    
    RealTimeDatabase db;
    public GameObject player;

    private void Start()
    {
        db = RealTimeDatabase.instance;
    }
    
    public void KillPlayer()
    {
        StatusDefault();

        LoadSence load = GetComponent<LoadSence>();
        load.LoadLevel(1);
    }

    public void StatusDefault()
    {
        db.DeleteInventory();
        db.DeleteEquipment();
        db.DeleteStatus();
    }
}