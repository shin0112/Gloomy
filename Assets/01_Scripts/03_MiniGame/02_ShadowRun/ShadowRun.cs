using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShadowRun : SceneSingletonManager<ShadowRun>
{
    public int EmotionPieceCount => emotionPieceCount;
    private int emotionPieceCount = 0;
    
    public bool IsPlaying => isPlaying;
    bool isPlaying = false;

    [SerializeField] private GameObject failUI;
    
    protected override void Init()
    {
        isPlaying = true;
        emotionPieceCount = 0;
    }


    public void AddEmotionPieceCount()
    {
        emotionPieceCount++;
    }

    public void ClearCheck()
    {
        isPlaying = false;
        if (emotionPieceCount >= 3)
        {
            Clear();
        }
        else
        {
            Fail();
        }
    }

    void Clear()
    {
        ChapterClearData.ClearChapter(2);
        SceneManager.LoadScene(Define.LibraryScene);
    }

    void Fail()
    {
        failUI.SetActive(true);
    }

    public void Replay()
    {
        SceneManager.LoadScene(Define.ShadowRunScene);
    }
}