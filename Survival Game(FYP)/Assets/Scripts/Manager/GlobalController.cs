using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalController : MonoBehaviour
{
    #region Singleton
    public static GlobalController instance;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }    
    }
    #endregion

    public List<ListItem> items = new List<ListItem>();
    public Equipment[] currentEquipment;
    public int health;
    public float thirst;
    public float hunger;
    public float[] position;


}
