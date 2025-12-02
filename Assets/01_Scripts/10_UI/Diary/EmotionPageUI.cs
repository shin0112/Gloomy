using System;
using UnityEngine;

public class EmotionPageUI : PageUI
{
    // todo : 감정 페이지 UI 내용 구성   

    [SerializeField] private GameObject[] emotionPieces;

    private void Awake()
    {
        int clearChapter = ChapterClearData.GetClearChapter();

        for (int i = 0; i < emotionPieces.Length; i++)
        {
            if (i <= clearChapter - 1)
            {
                emotionPieces[i].SetActive(true);
            }
            else
            {
                emotionPieces[i].SetActive(false);
            }
        }
    }
}