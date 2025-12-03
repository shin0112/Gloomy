using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IInteractable
{
    public void OnInteract();
    
    // 타게팅이 됐을 때, 벗어났을 때 할 동작 고려
    public void OnInteractionRangeEnter() { }
    public void OnInteractionRangeExit() { }
}

