using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using UnityEngine.Networking;
using System.Net.Http;
using System.Text;
using Firebase.Extensions;

public class RealTimeDatabase : MonoBehaviour
{
    #region Singalton
    public static RealTimeDatabase instance;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
    }
    #endregion

    public delegate void ChangeSceneCallBack();
    public ChangeSceneCallBack changeSceneCallBack;

    private const string projectId = "survival-game-firebase";
    private static readonly string databaseURL = $"https://survival-game-firebase-default-rtdb.firebaseio.com/";

    public static string localId { get; protected set; }
    public static string idToken { get; protected set; }
    public static string email { get; protected set; }

    public static bool IsLogin { get; protected set; }

    HttpClient client = new HttpClient();

    LoginMenu menu;
    // Start is called before the first frame update
    void Start()
    {
        menu = FindObjectOfType<LoginMenu>();
    }

    //Delete user data when they logout their account
    public void LogOut()
    {
        localId = "";
        idToken = "";
        email = "";
        IsLogin = false;
    }

    //Save user data after login of register successfully
    public void GetUserInfo(string _email, string _localId, string _idToken)
    {
        email = _email;
        localId = _localId;
        idToken = _idToken;
        IsLogin = true;
    }

    //Save Player Status to Firebase by the way Rest API (Android: Unverified, Windowns: OK)
    public void SaveStatusAPI(int health, float thirst, float hunger, float[] position)
    {
        UserData data = new UserData(health, thirst, hunger, position);
        string name = "Player Status";
        SaveData(name, data);       
    }

    //Save inventory to Firebase by the way Rest API (Android: Unverified, Windowns: OK)
    public void SaveInventoryAPI(List<int> inventory)
    {
        InventoryData data = new InventoryData(inventory);
        string name = "Inventory";
        SaveData(name, data);
    }

    //Save Equipment Data to Firebase by the way Rest API (Android: Unverified, Windowns: OK)
    public void SaveEquipmentAPI(List<equipment> equipments)
    {
        EquipementData data = new EquipementData(equipments);
        string name = "Equipments";
        SaveData( name, data);
    }

    //Save Building Data to Firebase by the way Rest API (Android: Unverified, Windowns: OK)
    public void SaveBuildingAPI(List<building> buildings)
    {
        buildingData data = new buildingData(buildings);
        string name = "Buildings";
        SaveData(name, data);
    }

    public void DeleteStatus()
    {
        DeleteData("Player Status");
    }

    public void DeleteInventory()
    {
        DeleteData("Inventory");
    }

    public void DeleteEquipment()
    {
        DeleteData("Equipments");
    }

    public void DeleteBuilding()
    {
        DeleteData("Buildings");
    }

    public void SaveData(string name, object data)
    {
        if(localId != null)
        {
            string json = JsonUtility.ToJson(data);
            var content = new StringContent(json, Encoding.ASCII, "application/json");
            client.PutAsync($"{databaseURL}/{localId}/{name}.json", content).ContinueWithOnMainThread(response => {
                if (response.Result.IsSuccessStatusCode)
                {
                    print("Save data successfully");
                }
                else
                {
                    Debug.Log("Save data Failed!");
                }
            });
        }
    }

    public string LoadData(string name)
    {
        string result = default;
        if(localId != null)
        {
            var response = client.GetAsync($"{databaseURL}/{localId}/{name}.json");
            if (response.Result.IsSuccessStatusCode)
            {
                result = response.Result.Content.ReadAsStringAsync().Result;
                print("Load Data Successfully!");
            }
            else
            {
                result = null;
                print("Load Data Failed!");
            }
        }
        return result;
    }

    public void DeleteData(string name)
    {
        if (localId != null)
        {
            client.DeleteAsync($"{databaseURL}/{localId}/{name}.json").ContinueWithOnMainThread(response => {
                if (response.Result.IsSuccessStatusCode)
                {
                    print("Delete data successfully");
                }
                else
                {
                    Debug.Log("Delete data Failed!");
                }
            });
        }
    }
}


