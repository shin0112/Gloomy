using System;
using TMPro;
using UnityEngine;

public class JournalPageUI : PageUI
{
    // todo : 일기장 페이지 UI 내용 구성
    [SerializeField] private GameObject journalPanel;
    [SerializeField] TMP_InputField journalTextInputField;

    private void Awake()
    {
        //
        //journalTextInputField.onValueChanged
    }

    protected override void OpenPageInternal()
    {
        if (firstOpen)
        {
            journalPanel.SetActive(true);
            journalTextInputField.gameObject.SetActive(true);
            journalTextInputField.ActivateInputField();
            journalTextInputField.Select();
        }
        else if (ChapterClearData.IsClear(2))
        {
            journalPanel.SetActive(true);
            journalTextInputField.gameObject.SetActive(true);
            journalTextInputField.text = "2챕터를 클리어했다!";
        }
        else
        {
            // todo : 추후 기획에 따라 변경 필요
            journalPanel.SetActive(false);
            journalTextInputField.gameObject.SetActive(false);
        }
    }
}
