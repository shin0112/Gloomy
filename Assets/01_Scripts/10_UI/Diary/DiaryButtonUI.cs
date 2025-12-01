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
    }


    void OnDiaryButtonClick()
    {
        if (isZoom == false)
        {
            // 확대하기
            StartCoroutine(DiaryButtonZoomInRoutine());
        }
        else
        {
            // 열기
            //
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
            StartCoroutine(DiaryButtonZoomOutRoutine());
        }
    }


    IEnumerator DiaryButtonZoomInRoutine()
    {
        diaryButton.interactable = false;
        float a = 0;

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
        backButton.interactable = false;
    }

}
