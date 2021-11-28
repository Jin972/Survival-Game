using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSence : MonoBehaviour
{
    #region Singleton
    public static LoadSence instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
    #endregion

    [SerializeField]
    private GameObject LoadingScreen;

    [SerializeField]
    [Tooltip("Content will display in Loading Screen")]
    private List<LoadingIntro> introduction;

    private Slider slider;

    private Text progressText;
    Text titleText;
    Text contentText;

    Transform ui;

    public int currentIndex {get; set; }

    private void Start()
    {
        slider = LoadingScreen.GetComponentInChildren<Slider>();
        progressText = LoadingScreen.transform.Find("PercentTxt").GetComponent<Text>();
        titleText = LoadingScreen.transform.Find("Title").GetComponent<Text>();
        contentText = LoadingScreen.transform.Find("Content").GetComponent<Text>();
        if(introduction.Count > 0)
        {
            int introIndex = Random.Range(0, introduction.Count);
            titleText.text = introduction[introIndex].title;
            contentText.text = introduction[introIndex].content;
        }
        else
        {
            titleText.text = "Movement";
            contentText.text = "Press <b>WASD<b> button to control character and use mouse to to adjust the viewing angle";
        }
    }

    void LateUpdate()
    {
        Canvas canvas = GameObject.FindWithTag("MainUI").GetComponent<Canvas>();
        if (canvas != null && ui == null)
        {
            ui = Instantiate(LoadingScreen, canvas.transform).transform;
            ui.gameObject.SetActive(false);
        }
    }

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
        currentIndex = SceneManager.GetActiveScene().buildIndex;
    }
    
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        ui.gameObject.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            progressText.text = progress * 100f + " %";
            yield return null;
        }
    }
}

[System.Serializable]
public class LoadingIntro
{
    public string title;
    public string content;
}
