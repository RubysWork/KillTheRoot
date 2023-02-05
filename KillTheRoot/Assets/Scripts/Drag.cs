using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{


    Vector3 offPos;
    Vector3 arragedPos;
    bool inBin = false;
    public int maxCountInBin = 5;

    private GameObject movingObj;
    GameStatusManager gameStatusManager;
    public string fullPath = "";

    private void Start()
    {
        gameStatusManager = FindObjectOfType<GameStatusManager>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "recycleBin")
        {
            inBin = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.name == "recycleBin")
        {
            inBin = false;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(transform.GetComponent<RectTransform>(), Input.mousePosition
     , eventData.enterEventCamera, out arragedPos))
        {
            offPos = transform.position - arragedPos;
            movingObj = Instantiate(this.gameObject, this.gameObject.GetComponentInParent<Transform>());
            movingObj.GetComponent<CanvasGroup>().alpha = 0.8f;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        movingObj.GetComponent<Transform>().position = offPos + Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (movingObj.GetComponent<Drag>().inBin && GameObject.Find("KeyboardController").GetComponent<KeyboardController>().spamInBinCount < maxCountInBin)
        {

            if (movingObj.tag == "spam")
            {
                Debug.Log("it's a spam");
                recycleSpam();
                if (gameStatusManager.gameStatus == GameStatus.FirstDay)
                {
                    EventBus.Publish(new ShowBossEmailEvent());
                }
            }
            else if (gameStatusManager.gameStatus == GameStatus.SecondDay && movingObj.tag == "bossEmail")//只有第二天的bossemail才有被计数
            {
                recycleEmail();
            }
            movingObj.GetComponent<Transform>().position = offPos + Input.mousePosition;
            gameObject.SetActive(false);
            movingObj.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            Destroy(movingObj);
            movingObj = null;
        }

    }
    void recycleSpam()
    {

        GameObject.Find("WindowGenerator").GetComponent<WindowGenerator>().spamCount--;
        // Debug.Log("remain" + GameObject.Find("WindowGenerator").GetComponent<WindowGenerator>().spamCount);

        int spamInBinCount = GameObject.Find("KeyboardController").GetComponent<KeyboardController>().spamInBinCount;
        // Debug.Log("spam in bin:" + spamInBinCount);
        if (spamInBinCount == 0)
        {
            GameObject.Find("recycleBin").GetComponent<Image>().sprite = Resources.Load(fullPath, typeof(Sprite)) as Sprite;//仅当进入回收站的spam从0到1时换图，我可以解释,你不必解释，太复杂了不想看
        }
        GameObject.Find("KeyboardController").GetComponent<KeyboardController>().spamInBinCount++;
        //show bin full
        if (GameObject.Find("KeyboardController").GetComponent<KeyboardController>().spamInBinCount == maxCountInBin)
        {
            GameObject.Find("FullBG").GetComponent<ShowBinFull>().Show();
        }

    }
    void recycleEmail()
    {
        //not count in spamInBin cause we dont want the email count to effect the judgment of the spam ending
        GameObject.Find("KeyboardController").GetComponent<KeyboardController>().emailInBinCount += 1;

        float emailInBin = GameObject.Find("KeyboardController").GetComponent<KeyboardController>().emailInBinCount;
        GameObject.Find("MadBoss").GetComponent<CanvasGroup>().alpha = emailInBin / (float)GameObject.Find("GameController").GetComponent<GameController>().rangeOfEmail;
        Debug.Log("BossAlpha:" + GameObject.Find("MadBoss").GetComponent<CanvasGroup>().alpha);
        GameObject.Find("MadBoss").GetComponent<Animator>().enabled = true;
        GameObject.Find("MadBoss").GetComponent<Animator>().Play("AngryFace", 0, 0);

    }

}