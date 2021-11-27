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

    private Slider slider;

    private Text progressText;
    Transform ui;

    public int currentIndex {get; set; }

    private void Start()
    {
        slider = LoadingScreen.GetComponentInChildren<Slider>();
        progressText = LoadingScreen.GetComponentInChildren<Text>();
        
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
