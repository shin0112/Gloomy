using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapPageUI : PageUI
{
    // todo : 지도 페이지 UI 내용 구성
    //[SerializeField] private List<Button> stageButton;
    [SerializeField] private Button stageButton;
    private void Awake()
    {
        stageButton.onClick.AddListener(LoadChapter);
        // for (int i = 0; i < stageButton.Count; ++i)
        // {
        //     int stageNum = i;
        //     stageButton[1].onClick.AddListener(() => { LoadStage(stageNum); }
        //     );
        // }
    }

    void LoadChapter()
    {
        // todo: 나중에 Scene 관리하는거 생기면 바꾸기
        //SceneManager.LoadScene(1);
        SceneManager.LoadScene("GameScene");
    }

}
