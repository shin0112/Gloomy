using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// 전에거 그대로 가져와서 고치는 중인데
// 가져오고보니 싱글톤 아니어도 되게따.,
public class CardMinigame : SceneSingletonManager<CardMinigame>
{
    
    [SerializeField] private float playTime = 30.0f;
    [SerializeField] CardBoard cardBoard;
    public TextMeshPro timeText;
    
    bool isPlaying = false;
    
    protected override void Init()
    {
        StartCoroutine(CountDown(StartGame));
    }

    void Update()
    {
        if(isPlaying == false)
            return;
        
        playTime -= Time.deltaTime;
        timeText.text = playTime.ToString("F2");

        if (playTime <= 0)
        {
            GameOver();
            return;
        }
        
        // todo : 나중에 Input 정리되면 해결하기
        // Input 여기다 일단 넣어!!!!

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                Card card = hit.collider.GetComponent<Card>();
                card?.OpenCard();
            }
        }
    }
    
    public void GameOver()
    {
        // Todo : 게임오버
        isPlaying = false;
        cardBoard.OpenAllCard();
        StartDelayAction(() =>
            {
                SceneManager.LoadScene(Define.CardGameScene);
            },
            3.0f);
    }
    
    
    public void GameClear()
    {   
        // Todo : 게임클레어 
        isPlaying = false;
        timeText.text = "CLEAR";
        StartDelayAction(() =>
            {
                SceneManager.LoadScene(Define.ShadowRunScene);
            },
            3.0f);
    }

    
    public void Replay()
    {
        SceneManager.LoadScene("Scene_Card");
        
    }

    void StartGame()
    { 
        cardBoard.SpawnCards();
        isPlaying =  true;
    }



    IEnumerator CountDown(Action action)
    {
        WaitForSeconds wait = new WaitForSeconds(1);
        
        timeText.text = "3";
        yield return wait;
        timeText.text = "2";
        yield return wait;
        timeText.text = "1";
        yield return wait;
        timeText.text = "START";
        yield return wait;
        action?.Invoke();
    }


}
