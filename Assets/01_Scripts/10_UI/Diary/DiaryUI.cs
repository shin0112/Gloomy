using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DiaryUI : MonoBehaviour
{

    [SerializeField] List<PageUI> pageUIPrefab;
    List<PageUI> pageUI = new List<PageUI>();

    //private PageUI nowOpenSheet; // 현재 페이지
    private int nowPageNum = 0;

    [SerializeField] Button nextButton;
    [SerializeField] Button prevButton;
    [SerializeField] private Transform leftPivot;
    [SerializeField] private Transform rightPivot;

    private void Awake()
    {
        for (int i = 0; i < pageUIPrefab.Count; i++)
        {
            PageUI newPage = Instantiate(pageUIPrefab[i], i % 2 == 0 ? leftPivot : rightPivot);
            pageUI.Add(newPage);
            newPage.gameObject.SetActive(false);
            //pageUIPrefab[i].gameObject.SetActive(false);
        }

        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PrevPage);
    }

    private void OnEnable()
    {
        ShowFirstPage();
    }

    /// <summary>
    /// 일기장 폈을 때 첫페이지부터 보기!
    /// </summary>
    public void ShowFirstPage()
    {

        OpenPage(0);

    }

    public void NextPage()
    {

        int nextPageNum = nowPageNum + 2;
        if (nextPageNum >= pageUIPrefab.Count)
        {
            // todo: 닫기

        }
        else
        {
            // todo: 다음페이지
            OpenPage(nextPageNum);
        }
    }

    public void PrevPage()
    {
        if (nowPageNum == 0) return;

        int nextPAgeNum = nowPageNum - 2;
        OpenPage(nextPAgeNum);
    }

    public void OpenPage(int pageNum)
    {
        pageUI[nowPageNum].gameObject.SetActive(false);
        if (pageUI.Count > nowPageNum + 1)
            pageUI[nowPageNum + 1].gameObject.SetActive(false);

        nowPageNum = pageNum;
        pageUI[nowPageNum].gameObject.SetActive(true);
        if (pageUI.Count > nowPageNum + 1)
            pageUI[nowPageNum + 1].gameObject.SetActive(true);

    }
}
