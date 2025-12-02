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

            if (ChapterClearData.IsChapterClearCheckInDaiary() == false)
            {
                newPage.SetFirstOpenAfterClear();
            }
        }

        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PrevPage);
    }

    private void OnEnable()
    {
        ShowFirstPage();
        
        // todo : 호출 지점 나중에 정리 필요할 수 있음
        ChapterClearData.CheckDiaryAfterClear();
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
        if (nextPageNum >= pageUI.Count)
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
        // 열려있던 페이지 닫기
        pageUI[nowPageNum].ClosePage();
        if (pageUI.Count > nowPageNum + 1)
            pageUI[nowPageNum + 1]?.ClosePage();

        // 현재 페이지 열기
        nowPageNum = pageNum;
        pageUI[nowPageNum].OpenPage();
        if (pageUI.Count > nowPageNum + 1)
            pageUI[nowPageNum + 1].OpenPage();

        
        // 다음, 이전 페이지 버튼 확인
        if (pageNum == 0)
            prevButton.gameObject.SetActive(false);
        else
            prevButton.gameObject.SetActive(true);
        
        if (pageNum + 2 >= pageUI.Count)
            nextButton.gameObject.SetActive(false);
        else
        {
            nextButton.gameObject.SetActive(true);
        }
    
    }
}
