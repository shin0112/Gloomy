using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

/// <summary>
///  지금은 Interaction 할 수 있는게 비석밖에 없어서 InteractableObject 만 뒀어요!
/// </summary>
public abstract class InteractableObject : MonoBehaviour, IInteractable
{
    /// <summary>
    /// 해당 오브젝트 타겟 됐을 때 상호작용 키 누르면 호출해주세요~
    /// </summary>
    public abstract void OnInteract();


    [SerializeField] private UnityEvent OnInteractionTargettedAction;
    [SerializeField]  private UnityEvent OnInteractionUntargettedAction;
    /// <summary>
    /// todo :  Player Ray 쪽에서 해야하지만 지금은 여기서 검사할게용
    /// </summary>
    public virtual void OnInteractionTargetted()
    {
        OnInteractionTargettedAction?.Invoke();
    }


    public virtual void OnInteractionUntargetted()
    {
        OnInteractionUntargettedAction?.Invoke();
    }

}
