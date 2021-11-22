using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildDetail : MonoBehaviour
{
    public Transform graphic;
    public Material red;
    public Material[] materials;
    Renderer[] transformChild;

    private void Start()
    {
        transformChild = graphic.GetComponentsInChildren<Renderer>();
        
        GetMaterial();
    }

    void GetMaterial()
    {
        int capital = transformChild.Length;
        materials = new Material[capital];
        for(int i = 0; i < transformChild.Length; i++)
        {
            materials[i] = transformChild[i].material;
        }
    }
}


