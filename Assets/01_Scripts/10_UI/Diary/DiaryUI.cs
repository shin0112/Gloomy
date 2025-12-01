using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DiaryUI : MonoBehaviour
{

    [SerializeField] List<PageUI> pageUI; // 페이지들 child 찾을거임
    private PageUI nowOpenPage;           // 현재 페이지
    private int nowPageNum = 0;

    [SerializeField] Button nextButton;
    [SerializeField] Button prevButton;

    private void Awake()
    {
        pageUI = GetComponentsInChildren<PageUI>().ToList();
        for (int i = 0; i < pageUI.Count; i++)
        {
            pageUI[i].gameObject.SetActive(false);
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
        nowPageNum = 0;

        if (nowOpenPage != null)
            nowOpenPage.gameObject.SetActive(false);

        pageUI[nowPageNum].gameObject.SetActive(true);
        nowOpenPage = pageUI[nowPageNum];
    }

    public void NextPage()
    {
        nowPageNum = Mathf.Clamp(nowPageNum + 1, 0, pageUI.Count - 1);
        nowOpenPage.gameObject.SetActive(false);
        
        if (nowPageNum == pageUI.Count)
        {
            // todo: 닫기

        }
        else
        {
            // todo: 다음페이지
            nowOpenPage = pageUI[nowPageNum];
            nowOpenPage.gameObject.SetActive(true);
        }
    }

    public void PrevPage()
    {
        nowPageNum = Mathf.Clamp(nowPageNum - 1, 0, pageUI.Count - 1);
        nowOpenPage.gameObject.SetActive(false);
        if (nowPageNum == 0)
        {
            nowOpenPage = pageUI[nowPageNum];
            nowOpenPage.gameObject.SetActive(true);
        }
    }
}
