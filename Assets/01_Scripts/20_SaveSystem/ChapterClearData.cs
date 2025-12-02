using UnityEngine;

/// <summary>
/// PlayerPrefs 저장 기반
/// todo :  추후 json으로도... 추가 저장 필요
/// todo : 종류 많아지면 PlayerPrefsManager 따로 만들어서 묶어도 됨
/// </summary>
public static class ChapterClearData
{
    const string ClearChapterKey = "ClearChapter";
    const string ChapterClearCheckDiaryKey = "ChapterClearCheckDiary";
    
    /// <summary>
    /// 챕터 넘버링은 게임 기준으로 합시다. 
    /// 1챕터 끝났으면 1 넘겨주세요
    /// </summary>
    /// <param name="chapter"></param>
    public static void ClearChapter(int chapter)
    {
        if (PlayerPrefs.HasKey(ClearChapterKey))
        {
            int clearChapter = PlayerPrefs.GetInt(ClearChapterKey);

            if (clearChapter > chapter)
                return;
        }

        PlayerPrefs.SetInt(ClearChapterKey, chapter);
        PlayerPrefs.SetInt(ChapterClearCheckDiaryKey, 0);
    }

    public static int GetClearChapter()
    {
        return PlayerPrefs.GetInt(ClearChapterKey, 0);
    }

    /// <summary>
    /// 챕터 클리어 후 다이어리 확인 했는지
    /// 이미 확인했으면 true / 확인 안했으면 false
    /// </summary>
    /// <returns></returns>
    public static bool IsChapterClearCheckInDaiary()
    {
        if (PlayerPrefs.HasKey(ChapterClearCheckDiaryKey))
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 챕터 클리어 후 다이어리 확인 했을 때 호출
    /// </summary>
    public static void CheckDiaryAfterClear()
    {
        if (PlayerPrefs.HasKey(ChapterClearCheckDiaryKey))
        {
            PlayerPrefs.DeleteKey(ChapterClearCheckDiaryKey);
        }
    }
    

    /// <summary>
    /// 특정 챕터 클리어 했는지?
    /// </summary>
    /// <param name="chapter"></param>
    /// <returns></returns>
    public static bool IsClear(int chapter)
    {
        // 프로토타입 챕터1 클리어상태로
        if (PlayerPrefs.HasKey(ClearChapterKey) == false)
            ChapterClearData.ClearChapter(1);
        
        if (PlayerPrefs.HasKey(ClearChapterKey))
        {
            int clearChapter = PlayerPrefs.GetInt(ClearChapterKey);
            if(clearChapter >= chapter)
                return true;
        }
        return false;
    }

    
    /// <summary>
    /// 챕터 세이브 데이터 삭제
    /// </summary>
    public static void RemoveSaveData()
    {
        PlayerPrefs.DeleteKey(ClearChapterKey);
        PlayerPrefs.DeleteKey(ChapterClearCheckDiaryKey);

        // 프로토타입 챕터1 클리어상태로
        PlayerPrefs.SetInt(ClearChapterKey, 1);
        
        PlayerPrefs.Save();
    }

}
