using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    int spamCountOnDesktop = 0;
    int spamCountInbin = 0;
    float emailInBin = 0;

    public int rangeOfSpam = 20;
    public int rangeOfEmail = 3;
    public bool flag = false;
    GameStatusManager gameStatusManager;

    public Image processBarFill;
    private float lastFrameValue;

    // Start is called before the first frame update
    void Start()
    {
        gameStatusManager = FindObjectOfType<GameStatusManager>();

    }
    void flagChange()
    {
        flag = true;
        Debug.Log("flag changed!");
    }
    void onSpamClear()
    {
        //彻底消灭广告木马
        Debug.Log("win!!");
        EventBus.Publish(new GameChange(GameStatus.GoodEnd));
    }

    void onSpamOverload()
    {
        Debug.Log("lose!!toomuch spam take your space of disk,ruining your critical data!!");
        EventBus.Publish(new GameChange(GameStatus.BadEnd1));


    }
    void onBossMad()
    {
        //bad end2
        Debug.Log("miss bossEmail&game over");
        EventBus.Publish(new GameChange(GameStatus.BadEnd2));
    }
    // Update is called once per frame
    void Update()
    {
        spamCountOnDesktop = GameObject.Find("WindowGenerator").GetComponent<WindowGenerator>().spamCount;
        spamCountInbin = GameObject.Find("KeyboardController").GetComponent<KeyboardController>().spamInBinCount;
        //critical debug 
        // Debug.Log("BossemailInBIN:" + emailInBin);
        // Debug.Log("count:" + GameObject.Find("WindowGenerator").GetComponent<WindowGenerator>().spamCount + "bin:" + GameObject.Find("KeyboardController").GetComponent<KeyboardController>().spamInBinCount);
        if (gameStatusManager.gameStatus == GameStatus.SecondDay)
        {
            if (!flag)
            {
                //延迟判定+tip2显示
                float period = GameObject.Find("WindowGenerator").GetComponent<WindowGenerator>().initiateTime * 2 + 0.2f;
                Invoke("flagChange", period);
            }

            if (flag)
            {
                //Debug.Log("judging");

                emailInBin = GameObject.Find("KeyboardController").GetComponent<KeyboardController>().emailInBinCount;
                if (spamCountOnDesktop == 0 && spamCountInbin == 0)
                {
                    onSpamClear();
                }

                if (spamCountInbin + spamCountOnDesktop > rangeOfSpam)
                {
                    onSpamOverload();
                }

                if (emailInBin > rangeOfEmail)
                {
                    onBossMad();
                }
            }
        }

        //processBar
        float spamProp = (float)(spamCountInbin + spamCountOnDesktop) / (float)rangeOfSpam;
        if (lastFrameValue - spamProp > 0.001)
        {

            processBarFill.fillAmount = lastFrameValue - 0.001f;
        }
        else if (lastFrameValue - spamProp < -0.001)
        {
            processBarFill.fillAmount = lastFrameValue + 0.001f;
        }

        lastFrameValue = processBarFill.fillAmount;
        // Debug.Log("lastvalue = " + lastFrameValue + "spams:" + (spamCountInbin + spamCountOnDesktop));
    }
}
