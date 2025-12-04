using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardBoardStone : InteractableObject
{
    [SerializeField] private PlayerController PlayerController;
    public override void OnInteract()
    {
        SceneManager.LoadScene(Define.CardGameScene);
        PlayerController.isOpenShadowScene = true;
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
