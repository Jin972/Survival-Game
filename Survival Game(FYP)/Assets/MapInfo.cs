using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapInfo : MonoBehaviour
{
    [SerializeField] 
    Transform iconTransform;
    [SerializeField]
    Transform buttonTransform;
    [SerializeField]
    GameObject currentLocalIcon;

    [SerializeField]
    GameObject iconPrefab;
    [SerializeField]
    GameObject buttonPrefab;

    [SerializeField]
    Canvas canvas;

    [SerializeField]
    Transform iconParent;
    [SerializeField]
    Transform btnParent;
    [SerializeField]
    Transform currentLocalParent;

    Transform iconUI;
    Transform btnUI;

    Transform cam;

    [SerializeField]
    int mapIndex = 1;

    [SerializeField]
    int currentMap = 1;

    LoadSence loadSence;

    public DangerousLevel dangerousLevel;

    [SerializeField]
    Sprite iconMain;

    // Start is called before the first frame update
    void Start()
    {
        loadSence = LoadSence.instance;
        cam = Camera.main.transform;
        iconUI = Instantiate(iconPrefab, iconParent).transform;
        btnUI = Instantiate(buttonPrefab, btnParent).transform;
        btnUI.gameObject.SetActive(false);

        if(mapIndex == loadSence.currentIndex)
        {
            print("current map " + mapIndex);
            currentLocalIcon.SetActive(true);
            currentLocalIcon.transform.position = currentLocalParent.position;
        }

        Image icon = iconUI.gameObject.GetComponent<Image>();
        Image iconchild = iconUI.Find("Image").GetComponent<Image>();
        if (iconchild != null && iconMain != null)
            iconchild.sprite = iconMain;

        switch (dangerousLevel) {
            case DangerousLevel.None:
                icon.color = new Color32(0, 192, 2, 255);
                break;
            case DangerousLevel.Normal:
                icon.color = new Color32(192, 185, 0, 255);
                break;
            case DangerousLevel.High:
                icon.color = new Color32(180, 18, 6, 255);
                break;
        }

        currentMap = loadSence.currentIndex;

        Button btn = btnUI.GetComponent<Button>();
        btn.onClick.AddListener(EnterMap);
    }

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            foreach (Transform child in btnParent)
            {
                child.gameObject.SetActive(false);
            }
            btnUI.gameObject.SetActive(true);
        }     
    }

    // Update is called once per frame
    void Update()
    {
        if (iconUI != null && btnUI != null)
        {
            iconUI.position = iconTransform.position;
            iconUI.forward = -cam.forward;

            btnUI.position = buttonTransform.position;

            currentLocalIcon.transform.position = currentLocalParent.position;
        }     
    }

    void EnterMap()
    {
        loadSence.LoadLevel(mapIndex);
    }
}

public enum DangerousLevel {None, Normal, High}