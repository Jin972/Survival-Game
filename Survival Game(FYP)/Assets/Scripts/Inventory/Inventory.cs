using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour, ISerializationCallbackReceiver
{
    #region Singleton
    public static Inventory instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than iten that found");
            return;
        }
        instance = this;
    }
    #endregion

    public PlayerStatistic database;
    string fileSave = "InventoryDataVr001.txt";

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int space = 20;
    public List<ListItem> items = new List<ListItem>();

    bool IsLoad = true;
    RealTimeDatabase db;
    private void Start()
    {
        db = RealTimeDatabase.instance;
        //Load();
        LoadData();
    }

    private void OnEnable()
    {
#if UNITY_EDITOR
        database = (PlayerStatistic)AssetDatabase.LoadAssetAtPath("Assets/Resources/InventoryDatabase.asset", typeof(PlayerStatistic));
#else 
        database = Resources.Load<PlayerStatistic>("InventoryDatabase");
#endif
    }

    public bool Add(Item item)
    {
        if (!item.isDefaultItem)
        {
            if (items.Count >= space)
            {
                Debug.Log("Not enough room.");
                return false;
            }
            items.Add(new ListItem(database.GetItemID[item], item));

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
            SaveData();
        }
        return true;
    }

    public void Remove(Item item)
    {
        ListItem list = items.Find(x => x.ID == database.GetItemID[item] && x.item == item);
        items.Remove(list);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
        SaveData();
    }

    public void Save()
    {
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, fileSave));
        bf.Serialize(file, saveData);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, fileSave)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, fileSave), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
        }
    }

    public void DeleteFile()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, fileSave)))
        {
            File.Delete(string.Concat(Application.persistentDataPath, fileSave));
            RefreshEditerProjectWindow();

            print("Deleted Inventory");
        }
        else
        {
            print("Deleted Inventory Fail");

        }
    }

    void RefreshEditerProjectWindow()
    {
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }

    public void OnAfterDeserialize()
    {
        //for (int i = 0; i < items.Count; i++)
        //{
        //    items[i].item = database.GetItem[items[i].ID];
        //}
    }

    public void OnBeforeSerialize()
    {
        
    }

    public void SaveData()
    {
        if(IsLoad == false)
        {
            List<int> tempItems = new List<int>();
            for (int i = 0; i < items.Count; i++)
            {
                tempItems.Add(items[i].ID);
            }
           
            db.SaveInventoryAPI(tempItems);
        }
    }

    public void LoadData()
    {
        IsLoad = true;
        try {
            string json = db.LoadData("Inventory");
            InventoryData extractedData = JsonUtility.FromJson<InventoryData>(json);
            for (int i = 0; i < extractedData.inventorys.Count; i++)
            {
                Add(database.GetItem[extractedData.inventorys[i]]);
            }
        } 
        catch {
            print("Nothing to load");
        }
        
        IsLoad = false;    
    }
}

[System.Serializable]
public class ListItem
{
    public int ID;

    public Item item;

    public ListItem(int id, Item item)
    {
        this.ID = id;
        this.item = item;
    }
}

