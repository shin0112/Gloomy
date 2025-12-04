using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardBoardStone : InteractableObject
{
    
    public override void OnInteract()
    {
        SceneManager.LoadScene(Define.CardGameScene);
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            OnInteractionTargetted();
        }
    }

    private void OnTriggerExit(Collider other)
    {       
        if (other.GetComponent<PlayerController>() != null)
        {
            OnInteractionUntargetted();
        }
    }
}
