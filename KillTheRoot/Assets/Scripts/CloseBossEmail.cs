using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseBossEmail : MonoBehaviour
{
    public void CloseWindow()
    {
        Destroy(gameObject);
    }
    public void DecreaseBossMad()
    {
        GameObject.Find("KeyboardController").GetComponent<KeyboardController>().emailInBinCount -= 0.5f;
        //display goodboss
        GameObject.Find("GoodBoss").GetComponent<Animator>().enabled = true;
        GameObject.Find("GoodBoss").GetComponent<Animator>().Play("AngryFace", 0, 0);
    }
    public void InCreaseBossMad()
    {
        GameObject.Find("KeyboardController").GetComponent<KeyboardController>().emailInBinCount += 0.5f;

        //display bad boss
        float emailInBin = GameObject.Find("KeyboardController").GetComponent<KeyboardController>().emailInBinCount;
        GameObject.Find("MadBoss").GetComponent<CanvasGroup>().alpha = emailInBin / (float)GameObject.Find("GameController").GetComponent<GameController>().rangeOfEmail;
        Debug.Log("BossAlpha:" + GameObject.Find("MadBoss").GetComponent<CanvasGroup>().alpha);
        GameObject.Find("MadBoss").GetComponent<Animator>().enabled = true;
        GameObject.Find("MadBoss").GetComponent<Animator>().Play("AngryFace", 0, 0);
    }
}
