using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

public class EquipmentManager : MonoBehaviour, ISerializationCallbackReceiver
{
    #region Singleton

    public static EquipmentManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public PlayerStatistic database;

    string fileSave = "Equipment.txt";

    public Equipment[] defaultItem;

    public GameObject TargetMeshTransform;

    public ListEquipment[] currentEquipment;
    public SkinnedMeshRenderer[] currentMeshes;

    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public event OnEquipmentChanged onEquipmentChanged;

    Inventory inventory;

    private BoneCombiner boneCombiner;

    private Transform rightHand;
    private Transform leftHand;

    public Transform RightHandHold;
    public Transform LeftHandHold;

    RealTimeDatabase db;
    bool IsLoad = true;

    void Start()
    {
        inventory = Inventory.instance;
        int numSlot = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new ListEquipment[numSlot];
        currentMeshes = new SkinnedMeshRenderer[numSlot];
        boneCombiner = new BoneCombiner(TargetMeshTransform);
        db = RealTimeDatabase.instance;
        EquipmentDefaultItem();
        //Load();
        LoadData();
    }

    private void OnEnable()
    {
#if UNITY_EDITOR
        database = (PlayerStatistic)AssetDatabase.LoadAssetAtPath("Assets/Resources/Database2.asset", typeof(PlayerStatistic));
#else 
        database = Resources.Load<PlayerStatistic>("Database2");
#endif
    }

    // Update is called once per frame
    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipSlot;
        Equipment oldItem =  Unequip(slotIndex);

        if (currentEquipment[slotIndex] != null)
        {
            if (currentEquipment[slotIndex].item != null)
            {
                oldItem = currentEquipment[slotIndex].item;
                inventory.Add(oldItem);
            }
        }
        // An item has been equipped so we trigger the callback
        if (onEquipmentChanged != null)
        onEquipmentChanged.Invoke(newItem, oldItem);

        currentEquipment[slotIndex] = new ListEquipment(database.GetEquipemtID[newItem], newItem);

        if(newItem.equipSlot == EquipmentSlot.Weapon)
        {
            rightHand = Instantiate(newItem.weapon, RightHandHold).transform;
        }
        else if(newItem.equipSlot == EquipmentSlot.Shield)
        {
            leftHand = Instantiate(newItem.weapon, LeftHandHold).transform;
        }
        else
        {
            SkinnedMeshRenderer newMesh = Instantiate<SkinnedMeshRenderer>(newItem.mesh);
            newMesh.transform.parent = TargetMeshTransform.transform;

            newMesh.bones = boneCombiner.GetBone(newItem.meshGameObject);
            newMesh.transform.localPosition = Vector3.zero;

            currentMeshes[slotIndex] = newMesh;
        }

        SaveData();
    }

    public Equipment Unequip(int slotIndex)
    {
        if (currentEquipment[slotIndex] != null )
        {
            if(currentEquipment[slotIndex].item != null)
            {
                if (currentMeshes[slotIndex] != null)
                {
                    Destroy(currentMeshes[slotIndex].gameObject);
                }
                else if(rightHand != null)
                {
                    Destroy(rightHand.gameObject);
                }
                else if (leftHand != null)
                {
                    Destroy(leftHand.gameObject);
                }
                    

                Equipment oldItem = currentEquipment[slotIndex].item;
                inventory.Add(oldItem);

                currentEquipment[slotIndex].item = null;
                currentEquipment[slotIndex].ID = 0;

                // Equipment has been removed so we trigger the callback
                if (onEquipmentChanged != null)
                    onEquipmentChanged.Invoke(null, oldItem);

                return oldItem;
            }      
        }
        return null;
    }

    public void UnequipAll()
    {
        for(int i = 0; i < currentEquipment.Length; i++)
        {
            Unequip(i);
        }
        EquipmentDefaultItem();

    }

    void EquipmentDefaultItem()
    {
        foreach (Equipment item in defaultItem)
        {
            Equip(item);
        }
    }

    void EquipmentAtRuntime()
    {
        for (int i = 0; i < currentEquipment.Length; i++)
        {
            if (currentEquipment[i] != null)
            {
                if(currentEquipment[i].item != null)
                {
                    print(currentEquipment[i].item);
                    Equip(currentEquipment[i].item);
                }
            
            }
            else
            {
                if (defaultItem.Length > 0 && defaultItem[i] != null)
                {
                    Equip(defaultItem[i]);
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UnequipAll();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            DeleteFile();
            print("deleted");
        }
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
            print("Load OK");
        }
        else
        {
            print("Load Fail");
        }
    }

    public void DeleteFile()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, fileSave)))
        {
            File.Delete(string.Concat(Application.persistentDataPath, fileSave));
            RefreshEditerProjectWindow();

            print("Deleted Equipment");
        }
        else
        {
            print("Deleted Equipment Fail");
           
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
        //for (int i = 0; i < currentEquipment.Length; i++)
        //{
        //    if (currentEquipment[i] != null)
        //    {
        //        if (currentEquipment[i].item != null)
        //        {
        //            Equip(database.GetEquipemt[currentEquipment[i].ID]);
        //        }
        //    }

        //}
        //LoadData();
    }

    public void OnBeforeSerialize()
    {
        
    }

    void SaveData()
    {
        if(IsLoad == false)
        {
            List<equipment> tempEquip = new List<equipment>();
            for (int i = 0; i < currentEquipment.Length; i++)
            {
                if (currentEquipment[i] != null)
                {
                    if (currentEquipment[i].item != null)
                    {
                        tempEquip.Add(new equipment(currentEquipment[i].ID, currentEquipment[i].item.name));
                    } 
                }
            }
            db.SaveEquipmentAPI(tempEquip);
        }
    }

    void LoadData()
    {
        IsLoad = true;
        try
        {
            string json = db.LoadData("Equipments");
            EquipementData extractedData = JsonUtility.FromJson<EquipementData>(json);
            for (int i = 0; i < extractedData.equipments.Count; i++)
            {
                if (extractedData.equipments[i].name != null)
                {
                    Equip(database.GetEquipemt[extractedData.equipments[i].equipID]);
                }
            }
        }
        catch
        {
            print("Nothing to load");
        }
        
        IsLoad = false;
    }
}

[System.Serializable]
public class ListEquipment
{
    public int ID;

    public Equipment item;

    public ListEquipment(int id, Equipment item)
    {
        this.ID = id;
        this.item = item;
    }
}



