using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PageUI : MonoBehaviour
{
    // 상속해서 쓸거임

    protected bool firstOpen = false;
    
    public void SetFirstOpenAfterClear()
    {
        firstOpen = true;   
    }

    public void OpenPage()
    {
        gameObject.SetActive(true);
        
        OpenPageInternal();
        
        firstOpen = false;
    }

    public void ClosePage()
    {
        gameObject.SetActive(false);
    }
    
    protected abstract void OpenPageInternal();
}
