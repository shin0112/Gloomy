using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionPieceShadowRun : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            ShadowRun.Instance.AddEmotionPieceCount();
            Destroy(this.gameObject);
        }

    }
}
