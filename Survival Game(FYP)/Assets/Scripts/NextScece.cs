using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScece : MonoBehaviour
{
    LoadSence loadSence;

    [SerializeField]
    string playerLayer = "Player";

    [SerializeField]
    int levelIndex = 1;

    private void Start()
    {
        loadSence = LoadSence.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(playerLayer))
            loadSence.LoadLevel(levelIndex); 
    }
}
