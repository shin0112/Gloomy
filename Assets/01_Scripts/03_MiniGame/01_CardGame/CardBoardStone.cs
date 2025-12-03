using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardBoardStone : InteractableObject
{
    public override void OnInteract()
    {
        SceneManager.LoadScene(Define.CardGameScene);
    }
}
