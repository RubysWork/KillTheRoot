using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagement : MonoBehaviour
{
    public List<AudioClip> audios = new List<AudioClip>();
    GameStatusManager statusManager;
    bool inGameBGMDoOnce = false;
    bool stopBGMDoOnce = false;
    void Start()
    {
        statusManager = FindObjectOfType<GameStatusManager>();
        this.GetComponent<AudioSource>().clip = audios[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (statusManager.gameStatus == GameStatus.FirstDay && !inGameBGMDoOnce)
        {
            this.GetComponent<AudioSource>().clip = audios[1];
            this.GetComponent<AudioSource>().Play();
            inGameBGMDoOnce = true;
        }
        if (statusManager.gameStatus == GameStatus.BadEnd1 ||
            statusManager.gameStatus == GameStatus.BadEnd2 ||
            statusManager.gameStatus == GameStatus.GoodEnd &&
            !stopBGMDoOnce)
        {
            this.GetComponent<AudioSource>().Stop();
            stopBGMDoOnce = true;
        }
    }
}
