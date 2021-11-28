using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStat : CharaterStats
{
    public float maxThirtst = 100;
    public float maxHunger = 100;

    public float thirstIncreaseRate = 0.05f;
    public float hungerIncreaseRate = 0.01f;

    public float thirst, hunger;

    public delegate void OnCurrentHealthChanged(int maxHealth, int currentHealth);
    public event OnCurrentHealthChanged onCurrentHealthChanged;

    public float lookRadius = 10f;

    private bool dead;

    string fileSave = "PlayerData.txt";

    private List<PlayerData> playerDatas = new List<PlayerData>();

    RealTimeDatabase db;

    bool isLoad = true;

    public override void Awake()
    {
        base.Awake();
        thirst = maxThirtst;
        hunger = maxHunger;
    }

    // Start is called before the first frame update
    void Start()
    {
        db = RealTimeDatabase.instance;
        EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
        //Load();
        LoadData();
    }

    private void Update()
    {
        if(!dead)
        {
            thirst -= thirstIncreaseRate * Time.deltaTime;
            hunger -= hungerIncreaseRate * Time.deltaTime;
        }    

        if(thirst <= 0f)
        {
            Die();
            print("You have die because of thirst.");
        }

        if(hunger <= 0f)
        {
            Die();
            print("You have die because of hunger.");
        }
    }

    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if(newItem != null)
        {
            armor.AddModifier(newItem.armorModifier);
            damage.AddModifier(newItem.damageModifier);
            attackRange.AddModifier(newItem.attckRangeModifier);
        }

        if(oldItem != null)
        {
            armor.RemoveModifier(oldItem.armorModifier);
            damage.RemoveModifier(oldItem.damageModifier);
            attackRange.RemoveModifier(oldItem.attckRangeModifier);
        }
    }

    public override void Die()
    {
        dead = true;
        base.Die();
        PlayerManager.instance.KillPlayer();
    }

    public void Drink(float value)
    {
        thirst += value;

        if (thirst >= 100)
            thirst = maxThirtst;
    }

    public void Eat(float value)
    {
        hunger += value;

        if (hunger >= 100f)
            hunger = maxHunger;
    }

    public void Healing(float value)
    {
        currentHealth += (int)value;

        if (currentHealth >= 100)
            currentHealth = maxHealth;

        if (onCurrentHealthChanged != null)
            onCurrentHealthChanged.Invoke(maxHealth, currentHealth);
    }

    // Save Data by local file
    public void Save()
    {
        AddData();
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, fileSave));
        bf.Serialize(file, saveData);
        file.Close();
        print("Save status successfully");
    }

    public void AddData()
    {
        playerDatas.Clear();

        float[] playerPosition = new float[3];
        playerPosition[0] = transform.position.x;
        playerPosition[1] = transform.position.y;
        playerPosition[2] = transform.position.z;
        playerDatas.Add(new PlayerData(currentHealth, thirst, hunger, playerPosition));
    }

    // Load Data in local file
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, fileSave)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, fileSave), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
            print("Load Status Successfully");
        }
    }
    
    //Delete Data in local file
    public void DeleteFile()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, fileSave)))
        {
            File.Delete(string.Concat(Application.persistentDataPath, fileSave));
            RefreshEditerProjectWindow();

            print("Deleted PLayer Status");
        }
        else
        {
            print("Delete PLayer Status Failed");
        }
    }

    void RefreshEditerProjectWindow()
    {
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }

    public void OnBeforeSerialize()
    {
        
    }

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < playerDatas.Count; i++)
        {
            currentHealth = playerDatas[i].health;
            thirst = playerDatas[i].thirst;
            hunger = playerDatas[i].hunger;
            Vector3 position;
            position.x = playerDatas[i].position[0];
            position.y = playerDatas[i].position[1];
            position.z = playerDatas[i].position[2];
            transform.position = position;
        }
    }

    //Save Data When User Turn Off Game
    private void OnApplicationQuit()
    {
        //Save();
        SaveData();
    }

    // Sava Data by Firebase
    public void SaveData()
    {
        if(isLoad == false && SceneManager.GetActiveScene().buildIndex != 2) // Check current map is "Main"(Home) scene or not
        {
            float[] position = new float[3];
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                
                position[0] = transform.position.x;
                position[1] = transform.position.y;
                position[2] = transform.position.z;
            }
            
            float _thirst = Mathf.Round(thirst);
            float _hunger = Mathf.Round(hunger);

            db.SaveStatusAPI(currentHealth, _thirst, _hunger, position);
        }
    }

    //Load Data by Firebase
    public void LoadData()
    {
        if(SceneManager.GetActiveScene().buildIndex != 2) //Check current map is "Map" scene or not
        {
            isLoad = true;
            try
            {
                string json = db.LoadData("Player Status");
                UserData data = JsonUtility.FromJson<UserData>(json);
                currentHealth = data.health;

                if (onCurrentHealthChanged != null)
                    onCurrentHealthChanged.Invoke(maxHealth, currentHealth);

                thirst = data.thirst;
                hunger = data.hunger;

                Vector3 position = transform.position;
                position.x = position[1];
                position.y = position[1];
                position.z = position[2];

                transform.position = position;
            }
            catch
            {
                print("Nothing to load");
            }

            isLoad = false;
        }
    }
        
}

[System.Serializable]
public class PlayerData
{
    public int health;
    public float thirst;
    public float hunger;
    public float[] position;

    public PlayerData (int health, float thirst, float hunger, float[] position)
    {
        this.health = health;
        this.thirst = thirst;
        this.hunger = hunger;
        this.position = position;
    }
}