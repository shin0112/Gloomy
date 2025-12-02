using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaryButtonUI : MonoBehaviour
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

        if (ChapterClearData.IsChapterClearCheckInDaiary())
        {
            clearParticle.SetActive(false);
        }
        else
        {
            clearParticle.SetActive(true);
        }
    }
    
    void OnDiaryButtonClick()
    {
        if (isZoom == false)
        {
            //StartCoroutine(DiaryButtonZoomInRoutine());
            
            Vector2 targetPos = Vector2.zero;
            Vector3 targetScale = Vector3.one * diaryScale;
            
            diaryButtonRect.anchoredPosition = targetPos;
            diaryButtonRect.localScale = targetScale;


            backButton.interactable = true;
            isZoom = true;
        }
        else
        {
            diaryUI.gameObject.SetActive(true);
            diaryButton.gameObject.SetActive(false);
            isOpen = true;
        }

    }

    void OnBackButtonClick()
    {
        if (isOpen == true)
        {
            isOpen = false;
            diaryUI.gameObject.SetActive(false);
            diaryButton.gameObject.SetActive(true);
        }
        
        if (isZoom == true)
        {
            Vector2 targetPos = diaryButtonOriginPos;
            Vector3 targetScale = Vector3.one;
            
            diaryButtonRect.anchoredPosition = targetPos;
            diaryButtonRect.localScale = targetScale;

            isZoom = false;
            backButton.interactable = false;
            //StartCoroutine(DiaryButtonZoomOutRoutine());
        }
    }

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

}
