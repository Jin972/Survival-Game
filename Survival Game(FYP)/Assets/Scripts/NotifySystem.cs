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
    private GameObject simpleNotifyObject;

    TMP_Text notifyContent;

    private void Start()
    {
        notifyContent = simpleNotifyObject.GetComponentInChildren<TMP_Text>();
    }

    public void SimpleNotify(string content)
    {
        print("content "  + content);
        if(content != null)
        {
            notifyContent.text = content;
            Instantiate(simpleNotifyObject, notifyParent);

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
}
