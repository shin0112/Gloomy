using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapPageUI : PageUI
{
    // todo : 지도 페이지 UI 내용 구성
    //[SerializeField] private List<Button> stageButton;
    [SerializeField] private List<ChapterSelectButton> chapterButtons;
   
    protected override void OpenPageInternal()
    {
        ActivateChapterButtons();
    }

    void ActivateChapterButtons()
    {
        for (int i = 0; i < chapterButtons.Count; i++)
        {
            chapterButtons[i].Init(i + 1);
        }
    }
}
