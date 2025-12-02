using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// 전에거 그대로 가져와서 고치는 중인데
// 가져오고보니 싱글톤 아니어도 되게따.,
public class CardMinigame : SceneSingletonManager<CardMinigame>
{
    
    [SerializeField] private float playTime = 30.0f;
    [SerializeField] CardBoard cardBoard;
    public TextMeshPro textTime;
    

    protected override void Init()
    {
        StartGame(); 
    }

    void Update()
    {
        if(Time.timeScale ==0.0f)
            return;
        
        playTime -= Time.deltaTime;
        textTime.text = playTime.ToString("F2");

        if (playTime <= 0)
        {
            Time.timeScale = 0;
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
        cardBoard.OpenAllCard();
    }
    
    
    public void GameClear()
    {   
        // Todo : 게임클레어 
    }

    
    public void Replay()
    {
        SceneManager.LoadScene("Scene_Card");
        
    }

    void StartGame()
    { 
        cardBoard.SpawnCards();
        Time.timeScale = 1;
        //StartCoroutine(StartGameCoroutine());
    }

    // IEnumerator StartGameCoroutine()
    // {
    //     Time.timeScale = 0;
    //     yield return new WaitForSeconds(3);
    //     Time.timeScale = 1;
    //     
    // }

}
