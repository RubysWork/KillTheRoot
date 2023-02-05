using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardController : MonoBehaviour
{
    public int spamInBinCount = 0;
    public float emailInBinCount = 0;
    public GameObject recycleBin;//如果在start中程序获取该物体会取不到（大概是实例化顺序问题）
    public string emptyPath = "Spirite/Bin_Empty";
    public Transform workPannel;
    GameStatusManager gameStatusManager;


    string[] reference = { "w", "a", "s", "d" };
    int toCheckIndex = 0;

    private void Start()
    {
        toInitialCheck();
        showClient1Only();

        gameStatusManager = FindObjectOfType<GameStatusManager>();
    }
    // public Sprite emptypic;
    void clearBin()
    {
        if (gameStatusManager.gameStatus == GameStatus.SecondDay)
        {
            spamInBinCount = 0;
            recycleBin.GetComponent<Image>().sprite = Resources.Load(emptyPath, typeof(Sprite)) as Sprite;//change spirite
            GameObject.Find("FullBG").GetComponent<ShowBinFull>().Hide();
        }

    }
    private void displayGoodBoss()
    {
        GameObject.Find("GoodBoss").GetComponent<Animator>().enabled = true;
        GameObject.Find("GoodBoss").GetComponent<Animator>().Play("AngryFace", 0, 0);
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            print("按下了E清空回收站,丝滑小连招断连");
            toInitialCheck();
            clearBin();
        }
        else if (Input.GetKeyDown(reference[toCheckIndex]))
        {
            showWord(reference[toCheckIndex]);
            print("按下了正确的下一个：" + reference[toCheckIndex]);

            if (toCheckIndex < 3)
            {
                print("下一个应该按：" + reference[toCheckIndex]);
                toCheckIndex++;
            }
            else if (toCheckIndex == 3)
            {
                if (emailInBinCount - 1 >= 0)
                {
                    emailInBinCount--;
                }
                print("连招检查完毕,全部正确，降低一个怒气值：" + emailInBinCount);
                //播放goodboss动画
                displayGoodBoss();
                StartCoroutine(replyToClient());
            }
        }
        else if (Input.anyKeyDown && !Input.GetMouseButtonDown(0))
        {
            print("既没按清空回收站也没按丝滑小连招,需要重新检查连招");
            toInitialCheck();
        }

    }
    public void initChatBox()
    {
        toInitialCheck();
        showClient1Only();
    }
    void toInitialCheck()
    {

        toCheckIndex = 0;
        //show tip
        workPannel.transform.Find("tip").gameObject.SetActive(true);
        //hide words
        for (int i = 0; i < reference.Length; i++)
        {
            GameObject obj = workPannel.transform.Find(reference[i]).gameObject;
            obj.SetActive(false);
        }
    }
    void showClient1Only()
    {
        workPannel.transform.Find("client1").gameObject.SetActive(true);
        workPannel.transform.Find("player").gameObject.SetActive(false);
        workPannel.transform.Find("client2").gameObject.SetActive(false);
    }

    void showWord(string word)
    {
        if (word == reference[0])
        {
            workPannel.transform.Find("tip").gameObject.SetActive(false);
        }
        workPannel.transform.Find(word).gameObject.SetActive(true);
    }
    IEnumerator replyToClient()
    {
        yield return new WaitForSeconds(0.8f);
        for (int i = 0; i < reference.Length; i++)
        {
            GameObject obj = workPannel.transform.Find(reference[i]).gameObject;
            obj.SetActive(false);
        }
        workPannel.transform.Find("player").gameObject.SetActive(true);

        yield return new WaitForSeconds(0.8f);
        workPannel.transform.Find("client2").gameObject.SetActive(true);

        yield return new WaitForSeconds(0.8f);
        workPannel.transform.Find("tip").gameObject.SetActive(true);
        toCheckIndex = 0;
        showClient1Only();

        StopCoroutine(replyToClient());
    }
}


