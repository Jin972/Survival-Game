using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObjects : MonoBehaviour
{

    public objectSort sort;
    public Material green;
    public Material red;

    private bool isSnapped = false;//only this script should change this value

    public bool isFoundation = false;

    public List<string> tagsISnapTo = new List<string>();

    public Transform graphics;

    private void OnTriggerEnter(Collider other)
    {
        for(int i = 0; i < tagsISnapTo.Count; i++)
        {
            string currentTap = tagsISnapTo[i];

            if(other.tag == currentTap)
            {
                transform.position = other.transform.position;
                isSnapped = true;
                ChangeColor();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < tagsISnapTo.Count; i++)
        {
            string currentTap = tagsISnapTo[i];

            if (other.tag == currentTap)
            {
                isSnapped = false;
                ChangeColor();
            }
        }
    }

    private void Update()
    {
            ChangeColor();

    }

    public void ChangeColor()
    {
        if (isSnapped)
        {
            foreach (Transform child in graphics)
            {
                child.GetComponent<Renderer>().material = green;
            }
        }
        else
        {
            foreach (Transform child in graphics)
            {
                child.GetComponent<Renderer>().material = red;
            }
        }

        if (isFoundation)
        {
            foreach (Transform child in graphics)
            {
                child.GetComponent<Renderer>().material = green;
            }
            isSnapped = true;
        }
    }

    public bool GetSnapped()//accessor for the isSnapped bool. 
    {
        return isSnapped;
    }
}

public enum objectSort {normal, foundation, floor }

