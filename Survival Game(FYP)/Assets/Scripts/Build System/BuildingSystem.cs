using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

public class BuildingSystem : MonoBehaviour, ISerializationCallbackReceiver
{
    #region Singleton

    public static BuildingSystem instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    string fileSave = "BuildingSystem.txt";
    public List<buildObjects> objects = new List<buildObjects>();
    public buildObjects currentObjects;
    private Vector3 currentPos;
    private Vector3 currentRot;

    public Transform buildingParent;
    public Transform currentPreview;
    Camera cam;
    public RaycastHit hit;
    public LayerMask layer;
    [SerializeField]
    LayerMask layDelete;
    RaycastHit oldHit = default;

    public float offset = 1f;
    public float gridSize = 1f;

    public bool IsBuilding;

    //public MCFace dir;

    public GameObject objectMenu;

    private bool chooseObj;

    public PlayerStatistic database;

    public List<BuildingData> buildingDatas = new List<BuildingData>();

    public int buildingID;

    public bool isRemove;

    [SerializeField]
    private CircleMenu menu;

    RealTimeDatabase db;

    bool IsLoad = false;

    private void Start()
    {
        db = RealTimeDatabase.instance;
        cam = Camera.main;
        //Load();
        LoadData();
    }

    private void OnEnable()
    {
#if UNITY_EDITOR
        database = (PlayerStatistic)AssetDatabase.LoadAssetAtPath("Assets/Resources/Database3.asset", typeof(PlayerStatistic));
#else 
        database = Resources.Load<PlayerStatistic>("Database3");
#endif
    }

    private void Update()
    {
        if (IsBuilding && !chooseObj && currentPreview != null)
            startPreview();

        if (Input.GetKeyDown(KeyCode.X))
        {
            StopPreview();

        }
        
        if (Input.GetButtonDown("ShowMenu"))
        {
            if (oldHit.point != default)
            {
                ColorDefault();
            }
            objectMenu.SetActive(!objectMenu.activeSelf);
            chooseObj = true;
            
        }

        if (Input.GetButtonDown("Fire2") && currentPreview != null)
        {
            print("building");
            if (menu.checkItemList(menu.buttons[menu.CurMenuItem].amount, menu.buttons[menu.CurMenuItem].itemRequest) == true)
            {
                Build();
                menu.ClearItems(menu.buttons[menu.CurMenuItem].itemRequest, menu.buttons[menu.CurMenuItem].amount);
            }
        }

        if (isRemove)
        {
            print("run");
            RemoveBuilding();
        }

    }

    public void TurnOffMenu()
    {
        objectMenu.SetActive(false);
        chooseObj = false;
    }

    public void ChangeCurrentBuilding(int cur)
    {
        currentObjects = objects[cur];
        if (currentPreview != null)
            Destroy(currentPreview.gameObject);

        GameObject curprev = (GameObject)Instantiate(currentObjects.preview, currentPos, Quaternion.Euler(currentRot));
        currentPreview = curprev.transform;
    }

    public void startPreview()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, layer))
        {
            if (hit.transform != this.transform)
                showPreview(hit);
        }
    }

    public void showPreview(RaycastHit _hit)
    {
        #region 
        //if(currentObjects.sort == objectSort.floor)
        //{
        //    dir = GetHitFace(_hit);
        //    if(dir == MCFace.Up || dir == MCFace.Down)
        //    {
        //        currentPos = _hit.point;
        //    }
        //    else
        //    {
        //        if (dir == MCFace.North)
        //            currentPos = _hit.point + new Vector3(0, 0, 2);

        //        if (dir == MCFace.South)
        //            currentPos = _hit.point + new Vector3(0, 0, -2);

        //        if (dir == MCFace.East)
        //            currentPos = _hit.point + new Vector3(2, 0, 0);

        //        if (dir == MCFace.West)
        //            currentPos = _hit.point + new Vector3(-2, 0, 0);
        //    }
        //}
        #endregion

        currentPos = _hit.point;
        currentPos -= Vector3.one * offset;
        currentPos /= gridSize;
        currentPos = new Vector3(Mathf.Round(currentPos.x), Mathf.Round(currentPos.y), Mathf.Round(currentPos.z));
        currentPos *= gridSize;
        currentPos += Vector3.one * offset;

        currentPreview.position = currentPos;

        if (Input.GetKeyDown(KeyCode.R))
            currentRot += new Vector3(0, 90, 0);

        currentPreview.localEulerAngles = currentRot;
    }

    private void StopPreview()//This is a bad name, It should be called something like BuildIt(). This will actually build your preview in the world
    {
        Destroy(currentPreview.gameObject);
        currentPreview = null;
    }

    public void Build()
    {
        PreviewObjects PO = currentPreview.GetComponent<PreviewObjects>();
        if (PO.GetSnapped())
        {
            Instantiate(currentObjects.prefab, currentPos, Quaternion.Euler(currentRot), buildingParent);

            float[] Position = new float[3];
            Position[0] = currentPos.x;
            Position[1] = currentPos.y;
            Position[2] = currentPos.z;

            float[] Rotation = new float[3];
            Rotation[0] = currentRot.x;
            Rotation[1] = currentRot.y;
            Rotation[2] = currentRot.z;
            buildingDatas.Add(new BuildingData(database.GetHouseItemID[currentObjects.prefab], currentObjects.prefab, Position, Rotation) );
            SaveData();
            //Save();
        }
    }

    void RemoveBuilding()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100, layDelete))
        {     
            if (hit.point != oldHit.point && oldHit.point != default)
            {
                ColorDefault();
            }

            oldHit = hit;
            BuildDetail PD = hit.transform.GetComponent<BuildDetail>();
            if (PD != null)
            {
                Renderer[] transforms = PD.graphic.GetComponentsInChildren<Renderer>();
                for (int i = 0; i < transforms.Length; i++)
                {
                    transforms[i].material = PD.red;
                }
            }

            if (Input.GetButtonDown("Fire1"))
            {
                Destroy(hit.transform.gameObject);
                ColorDefault();
            }
        }
    }

    void ColorDefault()
    {
        BuildDetail oldPO = oldHit.transform.GetComponent<BuildDetail>();
        if (oldPO != null)
        {
            Renderer[] transforms = oldPO.graphic.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < transforms.Length; i++)
            {
                transforms[i].material = oldPO.materials[i];
            }
        }
        oldHit = default;
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
        else
        {
            print("building Load fail");
        }
    }

    public void DeleteFile()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, fileSave)))
        {
            File.Delete(string.Concat(Application.persistentDataPath, fileSave));
            RefreshEditerProjectWindow();

            print("Deleted Building");
        }
        else
        {
            print("Deleted Building Fail");

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
        //for (int i = 0; i < buildingDatas.Count; i++)
        //{
        //    Vector3 position;
        //    position.x = buildingDatas[i].position[0];
        //    position.y = buildingDatas[i].position[1];
        //    position.z = buildingDatas[i].position[2];

        //    Vector3 rotation;
        //    rotation.x = buildingDatas[i].rotation[0];
        //    rotation.y = buildingDatas[i].rotation[1];
        //    rotation.z = buildingDatas[i].rotation[2];

        //    Instantiate(database.GetHouseItem[buildingDatas[i].id], position, Quaternion.Euler(rotation), buildingParent);
        //}        
    }

    void SaveData()
    {
        if(IsLoad == false)
        {
            List<building> tempBuild = new List<building>();
            for (int i = 0; i < buildingDatas.Count; i++)
            {
                tempBuild.Add(new building(buildingDatas[i].id, buildingDatas[i].position, buildingDatas[i].rotation));
            }

            db.SaveBuildingAPI(tempBuild);
        }
    }

    void LoadData()
    {
        IsLoad = true;
        try
        {
            string json = db.LoadData("Buildings");
            buildingData extractedData = JsonUtility.FromJson<buildingData>(json);

            for (int i = 0; i < extractedData.buildings.Count; i++)
            {
                Vector3 position;
                position.x = extractedData.buildings[i].position[0];
                position.y = extractedData.buildings[i].position[1];
                position.z = extractedData.buildings[i].position[2];

                Vector3 rotation;
                rotation.x = extractedData.buildings[i].rotation[0];
                rotation.y = extractedData.buildings[i].rotation[1];
                rotation.z = extractedData.buildings[i].rotation[2];

                Instantiate(database.GetHouseItem[extractedData.buildings[i].id], position, Quaternion.Euler(rotation), buildingParent);
            }
        }
        catch
        {
            print("Nothing to load!");
        }

        IsLoad = false;
    }

    //public static MCFace GetHitFace(RaycastHit _hit)
    //{
    //    Vector3 incomingVec = _hit.normal - Vector3.up;

    //    if (incomingVec == new Vector3(0, -1, -1))
    //        return MCFace.South;

    //    if (incomingVec == new Vector3(0, -1, 1))
    //        return MCFace.North;

    //    if (incomingVec == new Vector3(0, 0, 0))
    //        return MCFace.Up;

    //    if (incomingVec == new Vector3(1, 1, 1))
    //        return MCFace.Down;

    //    if (incomingVec == new Vector3(-1, -1, 0))
    //        return MCFace.West;

    //    if (incomingVec == new Vector3(1, -1, 0))
    //        return MCFace.East;

    //    return MCFace.None;
    //}
}

[System.Serializable]
public class buildObjects
{
    public string name;
    public GameObject prefab;
    public GameObject preview;
    public objectSort sort;
    public int gold;
}

//public enum MCFace { None, Up, Down, East, West, North, South }

[System.Serializable]
public class BuildingData
{
    public int id;

    public GameObject BuildingObject;

    public float[] position;

    public float[] rotation;

    public BuildingData(int id, GameObject BuildingObject, float[] position, float[] rotation)
    {
        this.id = id;
        this.BuildingObject = BuildingObject;
        this.position = position;
        this.rotation = rotation;
    }
}
