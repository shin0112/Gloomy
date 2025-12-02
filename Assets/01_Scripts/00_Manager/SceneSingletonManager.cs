using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 상속 예시!
/// public class CustomManager : SceneSingletonManager<CustomManager>
/// Awake 시점에 초기화 합니다. Instance는 Scene 내 배치되어있는 GameObject 기준 Start에서 접근해 주세요!
/// SceneSingletonManager 는 Scene 이동 시 Destroy 됩니다!
/// GlobalSingltonManager 는 Scene 이동 시에도 계속 남아있는 Singleton 이에요~
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class SceneSingletonManager<T> : MonoBehaviour where T : SceneSingletonManager<T>
{
    public static T Instance => instance;
    protected static T instance;
 

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = (T)this;
        
        Init();
    }

    /// <summary>
    /// Awake 타이밍에 동작합니다!
    /// 새로 만든 Manager에서 override 해서 쓰세요!
    /// </summary>
    protected virtual void Init()
    {
    }

    private void OnDestroy()
    {
        //안전하게 null 로 비우기
        instance = null;
    }
}
