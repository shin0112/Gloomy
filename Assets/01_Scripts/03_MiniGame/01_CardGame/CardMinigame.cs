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
    
    
    // [SerializeField]
    // private GameObject clearBoard;
    // [SerializeField]
    // private GameObject failBoard;


    protected override void Init()
    {
        Time.timeScale = 0;
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
        }
    }
    
    public void GameOver()
    {
        // Todo : 게임오버
        //failBoard.SetActive(true);
        cardBoard.OpenAllCard();
    }
    
    
    public void GameClear()
    {   
        // Todo : 게임클레어 
        //clearBoard.SetActive(true);
    }

    
    public void Replay()
    {
        SceneManager.LoadScene("Scene_Card");
        
    }

    void StartGame()
    { 
        //SoundManager.GetInstance().PlayBgm("bgmusic",true,1.0f);
        cardBoard.SpawnCards();   
    }

}
