using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScece : MonoBehaviour
{
    [SerializeField]
    string playerLayer = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer(playerLayer))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);        
    }
}
