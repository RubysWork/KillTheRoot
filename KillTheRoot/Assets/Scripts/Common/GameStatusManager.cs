using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStatusManager : MonoBehaviour
{

    public GameStatus gameStatus;
    [Header("Title")]
    public GameObject TitlePanel;
    [Header("FirstDay")]
    public GameObject StartDayPanel;

    [Header("SecondDay")]
    public GameObject tip2;
    public Sprite img;

    [Header("EndFirstDay")]
    public GameObject DeskTopPanel;
    public GameObject EndFirstDayPanel;

    [Header("End")]
    public GameObject EndPanel;
    public GameObject GoodEnd;
    public GameObject BadEnd1;
    public GameObject BadEnd2;

    bool StartFirstDoOnce = false;
    bool EndFirstDoOnce = false;
    bool EndSecondDoOnce = false;
    bool GoodEndDoOnce = false;
    bool BadEnd1DoOnce = false;
    bool BadEnd2DoOnce = false;
    Subscription<GameChange> gamechangeSub;


    private void Start()
    {
        gameStatus = GameStatus.Title;
        gamechangeSub = EventBus.Subscribe<GameChange>(OnGameChange);
    }
    private void Update()
    {
        switch (gameStatus)
        {
            case (GameStatus.Title):

                break;
            case (GameStatus.FirstDay):
                if (!StartFirstDoOnce)
                {
                    StartCoroutine(FirstDayAnimation());
                    StartFirstDoOnce = true;
                }
                break;
            case (GameStatus.EndFirstDay):
                if (!EndFirstDoOnce)
                {
                    StartCoroutine(PlayEndFirstDayAnimation());
                    EndFirstDoOnce = true;
                }
                break;
            case (GameStatus.SecondDay):
                if (!EndSecondDoOnce)
                {

                    EndSecondDoOnce = true;
                }
                break;
            case (GameStatus.GoodEnd):
                if (!GoodEndDoOnce)
                {
                    EndPanel.SetActive(true);
                    GoodEnd.SetActive(true);
                    GoodEndDoOnce = true;
                }
                break;
            case (GameStatus.BadEnd1):
                if (!BadEnd1DoOnce)
                {
                    EndPanel.SetActive(true);
                    BadEnd1.SetActive(true);
                    BadEnd1DoOnce = true;
                }
                break;
            case (GameStatus.BadEnd2):
                if (!BadEnd2DoOnce)
                {
                    EndPanel.SetActive(true);
                    BadEnd2.SetActive(true);
                    BadEnd2DoOnce = true;
                }
                break;
        }
    }

    public void StartGame()
    {
        StartCoroutine(StartGameAni());
    }
    void OnGameChange(GameChange gameChange)
    {
        gameStatus = gameChange.gameState;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    IEnumerator StartGameAni()
    {
        TitlePanel.GetComponent<Fade>().enabled = true;
        yield return new WaitForSeconds(1);
        TitlePanel.SetActive(false);
        EventBus.Publish(new GameChange(GameStatus.FirstDay));
    }
    IEnumerator FirstDayAnimation()
    {
        StartDayPanel.SetActive(true);
        yield return new WaitForSeconds(2);
        DeskTopPanel.GetComponent<CanvasGroup>().alpha = 1;
        StartDayPanel.GetComponent<Fade>().fadeIn = false;
        StartDayPanel.GetComponent<Fade>().enabled = true;

        yield return new WaitForSeconds(2);
        StartDayPanel.SetActive(false);

        StopCoroutine(FirstDayAnimation());

    }

    IEnumerator PlayEndFirstDayAnimation()
    {
        DeskTopPanel.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(0.5f);

        DeskTopPanel.GetComponent<Animator>().enabled = false;
        EndFirstDayPanel.SetActive(true);
        yield return new WaitForSeconds(3f);

        tip2.SetActive(true);

        DeskTopPanel.GetComponent<Animator>().SetFloat("Speed", -1);
        DeskTopPanel.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(0.5f);
        DeskTopPanel.GetComponent<Animator>().enabled = false;
        DeskTopPanel.transform.Find("Desktop").GetComponent<Image>().sprite = img;
        //在开始第二天的时候预先生成几个
        GameObject.Find("WindowGenerator").GetComponent<WindowGenerator>().preGenerateSpams(5);
        GameObject.Find("KeyboardController").GetComponent<KeyboardController>().initChatBox();


        EndFirstDayPanel.GetComponent<Fade>().fadeIn = false;
        EndFirstDayPanel.GetComponent<Fade>().enabled = true;
        DeskTopPanel.GetComponent<Fade>().enabled = true;
        yield return new WaitForSeconds(3f);
        EndFirstDayPanel.SetActive(false);

        yield return new WaitForSeconds(0.5f);


        EventBus.Publish(new GameChange(GameStatus.SecondDay));

        StopCoroutine(PlayEndFirstDayAnimation());
    }
}
