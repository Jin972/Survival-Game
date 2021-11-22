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
        instance = this;
    }
    #endregion

    [SerializeField]
    private GameObject LoadingScreen;
    
    private Slider slider;
    
    private Text progressText;

    private void Start()
    {
        slider = LoadingScreen.GetComponentInChildren<Slider>();
        progressText = LoadingScreen.GetComponentInChildren<Text>();
    }

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }
    
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        LoadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            progressText.text = progress * 100f + " %";
            yield return null;
        }
    }
}
