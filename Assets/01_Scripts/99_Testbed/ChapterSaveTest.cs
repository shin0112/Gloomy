using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChapterSaveTest : MonoBehaviour
{
    
}

#if UNITY_EDITOR

[CustomEditor(typeof(ChapterSaveTest))]
public class CphaterSaveTest : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        GUILayout.Space(10);
        GUILayout.Label("챕터 클리어 테스트");

        if (GUILayout.Button("2챕 클리어"))
        {
            Logger.Log("2챕 클리어!");
            ChapterClearData.RemoveSaveData();
            ChapterClearData.ClearChapter(2);
        }

        if (GUILayout.Button("세이브 데이터 삭제"))
        {
            Logger.Log("챕터 클리어 데이터 삭제!");
            ChapterClearData.RemoveSaveData();
        }
        
    }
    
    
    
}

#endif

