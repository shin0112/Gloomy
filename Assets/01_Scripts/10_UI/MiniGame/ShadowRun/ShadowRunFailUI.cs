using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 임시
/// 나중에 대화창 진하게 만들 때 작업하기
/// </summary>
public class ShadowRunFailUI : MonoBehaviour
{
    [SerializeField] private Button replayButton;

    private void Awake()
    {
        replayButton.onClick.AddListener(() =>
        {
            ShadowRun.Instance.Replay();
        });
    }
}