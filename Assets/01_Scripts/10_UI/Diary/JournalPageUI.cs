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
            journalTextInputField.gameObject.SetActive(true);
            journalTextInputField.ActivateInputField();
            journalTextInputField.Select();
        }
        else
        {
            // todo : 추후 기획에 따라 변경 필요
            journalTextInputField.gameObject.SetActive(false);
        }
    }
}
