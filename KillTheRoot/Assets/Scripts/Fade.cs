using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public float fadeSpeed = 0.05f;
    public bool fadeIn=true;

    bool finishFadeIn = false;
    bool finishFadeOut = false;

    CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup =  this.GetComponent<CanvasGroup>();
    }
    void Update()
    {
        if (fadeIn&&!finishFadeIn) { FadeIn(); }
        if (!fadeIn&&!finishFadeOut){ FadeOut(); }
    }

    void FadeIn() 
    {
        canvasGroup.alpha+= fadeSpeed;
        if (canvasGroup.alpha >= 1) 
        {
            canvasGroup.alpha = 1;
            finishFadeIn = true;
            this.GetComponent<Fade>().enabled = false;
        }
    }
    void FadeOut() 
    {
        canvasGroup.alpha -= fadeSpeed;
        if (canvasGroup.alpha <= 0)
        {
            canvasGroup.alpha = 0;
            finishFadeOut = true;
            this.GetComponent<Fade>().enabled = false;
        }
    }
}
