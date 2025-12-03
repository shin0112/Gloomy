using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChapterSelectButton : MonoBehaviour
{
    [SerializeField] private GameObject cloud;
    [SerializeField] private TextMeshProUGUI chapterText;
    private int chpater;
    private Button button;

    public void Init(int _chpater)
    {
        chpater= _chpater;  
        
        chapterText.text = _chpater.ToString();
        button = GetComponent<Button>();
        button.onClick.AddListener(LoadChapter);
        
        if(ChapterClearData.IsClear(chpater-1) == true)
            OpenChapter();
        else
            CloseChapter();
    }
    
    public void OpenChapter()
    {
        cloud.SetActive(false);
    }

    public void CloseChapter()
    {
        cloud.SetActive(true);
    }
    
    void LoadChapter()
    {
        // todo: 나중에 Scene 관리하는거 생기면 바꾸기
        SceneManager.LoadScene(Define.CardGameEnterScene);
    }
}
