using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnimator : MonoBehaviour
{  
    //Sprite Animator
    [SerializeField]
    private Sprite[] frameArray;
    private int currentFrame;
    private float timer;
    private float framerate = .1f;
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = gameObject.GetComponent<Image>();              
    }

    
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= framerate)
        {
            timer -= framerate;
            currentFrame = (currentFrame + 1) % frameArray.Length;
            image.sprite = frameArray[currentFrame];
        }
    }
}
