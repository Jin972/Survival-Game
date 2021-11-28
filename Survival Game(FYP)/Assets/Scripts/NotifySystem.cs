using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotifySystem : MonoBehaviour
{
    #region Singleton
    public static NotifySystem instance;

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

    [SerializeField]
    private Transform notifyParent;

    [SerializeField]
    private GameObject simpleNotifyPrefab;
    TMP_Text notifyContent;

    [SerializeField]
    GameObject confirmDialogPrefab;
    TMP_Text confirmTitle;
    TMP_Text confirmContent;
    Button cancelBtn;
    Button confirmBtn;

    Transform confirmUI;

    LoadSence loadSence;

    InventoryUI ui;

    private void Start()
    {
        loadSence = LoadSence.instance;
        ui = GameObject.FindGameObjectWithTag("MainUI").GetComponent<InventoryUI>();

        if (notifyParent == null) {
            Transform canvas = GameObject.FindWithTag("MainUI").GetComponent<Transform>();
            notifyParent = canvas.Find("NotifySystem").GetComponent<Transform>();
        }
        notifyContent = simpleNotifyPrefab.GetComponentInChildren<TMP_Text>();

        //Get components of Confirm Dialog
        confirmTitle = confirmDialogPrefab.transform.Find("Title").GetComponent<TMP_Text>();
        confirmContent = confirmDialogPrefab.transform.Find("Content").GetComponent<TMP_Text>();
    }

    private void LateUpdate()
    {
        if (notifyParent == null)
        {
            Transform canvas = GameObject.FindWithTag("MainUI").GetComponent<Transform>();
            notifyParent = canvas.Find("NotifySystem").GetComponent<Transform>();
        }
    }

    public void SimpleNotify(string content)
    {
        print("content "  + content);
        if(content != null)
        {
            notifyContent.text = content;
            Instantiate(simpleNotifyPrefab, notifyParent);

            StartCoroutine(DestroyNotify(2f));
        }
    }

    IEnumerator DestroyNotify(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (Transform child in notifyParent)
        {
            Destroy(child.gameObject);
        }
    }

    public void ConfirmDialogChangeMap()
    {
        confirmTitle.text = "Confirm";
        confirmContent.text = "Are you sure want to exit this map?";
        
        confirmUI = Instantiate(confirmDialogPrefab, notifyParent).transform;

        cancelBtn = confirmUI.transform.Find("CancelBtn").GetComponent<Button>();
        confirmBtn = confirmUI.transform.Find("ConfirmBtn").GetComponent<Button>();

        cancelBtn.onClick.AddListener(Cancel);
        confirmBtn.onClick.AddListener(ChangeMap);
        if(ui != null)
        {
            ui.UINumber += 1;
        }
    }

    public void Cancel()
    {
        print("test cancel button");
        if(confirmUI != null)
        {
            Destroy(confirmUI.gameObject);
            if (ui != null)
            {
                ui.UINumber -= 1;
                if (ui.UINumber < 0)
                {
                    ui.UINumber = 0;
                }
            }
        }
    }

    public void ChangeMap()
    {
        loadSence.LoadLevel(2);

        Destroy(confirmUI.gameObject);
        if (ui != null)
        {
            ui.UINumber -= 1;
            if (ui.UINumber < 0)
            {
                ui.UINumber = 0;
            }
        }
    }
}
