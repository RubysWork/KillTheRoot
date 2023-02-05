using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WindowGenerator : MonoBehaviour
{
    //3s move to bin

    public float initiateTime = 2;
    public float bossEmailTime = 5;
    public void onModeChanged(Slider slider)
    {
        initiateTime = 6f * slider.value;
        bossEmailTime = 10f * slider.value;
        if (initiateTime <= 0) initiateTime = 0.2f;
        if (bossEmailTime <= 0) bossEmailTime = 0.3f;
        Debug.Log(slider.value);
    }
    public GameObject spam;
    //used in GameController and others
    public int spamCount = 0;
    public GameObject email;

    bool isRepeat1 = false;
    bool isRepeat2 = false;
    bool isRepeat3 = false;
    public GameObject icon;
    GameObject firstdayEmail;

    GameStatusManager gameStatusManager;
    Subscription<ShowBossEmailEvent> showbossSub;

    void Start()
    {
        gameStatusManager = FindObjectOfType<GameStatusManager>();
        showbossSub = EventBus.Subscribe<ShowBossEmailEvent>(ShowBossEmail);

    }
    void updateIcon()
    {
        if (icon != null)
        {
            if (spamCount > 0)
            {
                icon.SetActive(true);
            }
            else
            {
                icon.SetActive(false);

            }
        }

    }
    private void Update()
    {
        updateIcon();

        if (!isRepeat1)
        {
            if (gameStatusManager.gameStatus == GameStatus.FirstDay)
            {
                StartCoroutine(FirstDay());
                isRepeat1 = true;
            }

        }

        if (!isRepeat2)
        {
            if (gameStatusManager.gameStatus == GameStatus.SecondDay)
            {
                //bossEmailTime
                InvokeRepeating("InitiateSpam", 0, initiateTime);
                InvokeRepeating("InitiateBoss", bossEmailTime, bossEmailTime);
                isRepeat2 = true;
            }
        }

        if (!isRepeat3)
        {
            if (gameStatusManager.gameStatus == GameStatus.GoodEnd || gameStatusManager.gameStatus == GameStatus.BadEnd1 || gameStatusManager.gameStatus == GameStatus.BadEnd2)
            {
                CancelInvoke("InitiateSpam");
                CancelInvoke("InitiateBoss");
                isRepeat3 = true;
            }
        }
    }
    public void preGenerateSpams(int c)
    {
        for (int i = 0; i < c; i++)
        {
            InitiateSpam();
        }
    }
    void InitiateSpam()
    {
        GameObject obj = GameObject.Instantiate(spam);
        obj.transform.SetParent(GameObject.Find("DesktopPanel").transform);
        obj.tag = "spam";
        obj.transform.localPosition = RandomizePosition();
        spamCount++;
    }
    void InitiateBoss()
    {
        GameObject obj = GameObject.Instantiate(email);
        obj.transform.SetParent(GameObject.Find("DesktopPanel").transform);
        obj.tag = "bossEmail";
        obj.transform.localPosition = RandomizePosition();

        if (gameStatusManager.gameStatus == GameStatus.FirstDay)
        {
            firstdayEmail = obj;
        }
    }
    Vector3 RandomizePosition()
    {
        float x = Random.Range(-140, 140);
        float y = Random.Range(-40, 40);
        Vector3 position = new Vector3(x, y, 0);
        return position;
    }

    void ShowBossEmail(ShowBossEmailEvent showBossEmailEvent)
    {
        StartCoroutine(ShowBoss());
    }
    IEnumerator FirstDay()
    {

        yield return new WaitForSeconds(initiateTime);
        GameObject obj = GameObject.Instantiate(spam);
        obj.transform.SetParent(GameObject.Find("DesktopPanel").transform);
        obj.tag = "spam";
        obj.transform.localPosition = RandomizePosition();
        spamCount++;

        StopCoroutine(FirstDay());
    }

    IEnumerator ShowBoss()
    {
        InitiateBoss();

        yield return new WaitForSeconds(1.5f);
        Destroy(firstdayEmail);
        EventBus.Publish(new GameChange(GameStatus.EndFirstDay));
        StopCoroutine(ShowBoss());
    }
}
