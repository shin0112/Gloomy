using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaryButton : MonoBehaviour
{
    [SerializeField] private Button diaryButton;
    [SerializeField] private Button backButton;
    [SerializeField] private DiaryUI diaryUI;
    [SerializeField] private GameObject clearParticle;

    private bool isZoom;
    private bool isOpen;

    private Coroutine diaryZoomRoutine;

    RectTransform diaryButtonRect;

    private float speed = 2.0f;
    private float diaryScale = 5f;
    
    private Vector2 diaryButtonOriginPos;
    private Vector2 diaryButtonCenterPos = Vector2.zero;

    
    private void Awake()
    {
        diaryButton.interactable = true;
        backButton.interactable = false;

        diaryButton.onClick.AddListener(OnDiaryButtonClick);
        backButton.onClick.AddListener(OnBackButtonClick);

        diaryButtonRect = diaryButton.GetComponent<RectTransform>();
        diaryButtonOriginPos = diaryButtonRect.anchoredPosition;

        ChpaterClearCheck();
    }
    
    void OnDiaryButtonClick()
    {
        if (isZoom == false)
        {
            ZoomDiaryButton(true);
        }
        else  
        {
            // 일기장 오픈
            OpenDiary(true);
        }

    }
    void OnBackButtonClick()
    {
        if (isOpen == true)
        {
            ZoomDiaryButton(false);
            OpenDiary(false);
        }
        else if (isZoom == true)
        {
            ZoomDiaryButton(false);
        }
    }

    

    public void OpenDiary(bool _isOpen)
    {
        isOpen = _isOpen;
        diaryUI.gameObject.SetActive(isOpen);
        diaryButton.gameObject.SetActive(!isOpen);
    }

    public void ZoomDiaryButton(bool _isZoom)
    {
        isZoom = _isZoom;

        Vector2 targetPos = isZoom? Vector2.zero : diaryButtonOriginPos;
        Vector3 targetScale = isZoom? Vector3.one * diaryScale : Vector3.one;
        
        diaryButtonRect.anchoredPosition = targetPos;
        diaryButtonRect.localScale = targetScale;
      
        backButton.interactable = isZoom;
        ChpaterClearCheck();
    }

    public void ChpaterClearCheck()
    {
        if (isZoom == false)
        {
            // 다이어리 확인
            if (ChapterClearData.IsChapterClearCheckInDaiary())
            {
                clearParticle.SetActive(false);
            }
            else
            {
                clearParticle.SetActive(true);
            }
        }
        else
        {
            clearParticle.SetActive(false);
        }
    }
    
    #region 확대축소 애니메이션 (햔제 미사용)

    // todo : 합칠 수 있을 때 합치기
    // todo : 하다 말았던 PresetChange Extension 추가해보기
    IEnumerator DiaryButtonZoomInRoutine()
    {
        diaryButton.interactable = false;
        backButton.interactable = false;
        float a = 0;
        //
        // diaryButtonRect.SetAnchorPreset(AnchorPreset.TopLeft, PivotPreset.TopLeft);

        Vector2 startPos = diaryButtonRect.anchoredPosition;
        Vector2 targetPos = Vector2.zero;
        
        Vector3 startScale = diaryButtonRect.localScale;
        Vector3 targetScale = Vector3.one * diaryScale;

        while (a < 1)
        {
            a += (Time.deltaTime * speed);
            diaryButtonRect.anchoredPosition = Vector2.Lerp(startPos, targetPos, a);
            diaryButtonRect.localScale = Vector3.Lerp(startScale,targetScale, a);
            yield return null;
        }
        
        isZoom = true;
        diaryButton.interactable = true;
        backButton.interactable = true;
    }

    IEnumerator DiaryButtonZoomOutRoutine()
    {
        backButton.interactable = false;
        diaryButton.interactable = false;
        float a = 0;
        
        
        Vector2 startPos = diaryButtonRect.anchoredPosition;
        Vector2 targetPos = diaryButtonOriginPos;

        Vector3 startScale = diaryButtonRect.localScale;
        Vector3 targetScale = Vector3.one;
        
        while (a < 1)
        {
            a += (Time.deltaTime * speed);
            diaryButtonRect.anchoredPosition = Vector2.Lerp(startPos, targetPos, a);
            diaryButtonRect.localScale = Vector3.Lerp(startScale,targetScale, a);
            yield return null;
        }
        
        isZoom = false;
        diaryButton.interactable = true;
        backButton.interactable = true;
    }


        #endregion
 
}
