using System;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int idx { get; private set; }

    [SerializeField] GameObject front;
    [SerializeField] GameObject back;
    [SerializeField] Renderer backRenderer;

    public event Action OnOpenCardAction; 

    public void Init(int _idx, Sprite _sprite)
    {
        idx = _idx;
     
        // Sprite spr = Resources.Load<Sprite>("Sprite/" + "rtan" + idx.ToString());
        // back.GetComponent<Image>().sprite = spr;
        
        backRenderer.material.mainTexture = _sprite.texture;
        back.gameObject.SetActive(false);
    }

    public void OpenCard()
    {
        //SoundManager.instance.PlayOnce("flip");
        front.SetActive(false);
        back.SetActive(true);
        
        OnOpenCardAction?.Invoke();
    }

    public void CloseCard()
    {
        back.SetActive(false);
        front.SetActive(true);
    }

    public void ForceOpenCard()
    {
        front.SetActive(false);
        back.SetActive(true);
    }

    // public void SetCardSize(float width, float height)
    // {
    //     front.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    //     if (width < height)
    //     {
    //         back.GetComponent<RectTransform>().sizeDelta = new Vector2(width, width);
    //     }
    //     else
    //     {
    //         back.GetComponent<RectTransform>().sizeDelta = new Vector2(height, height);
    //     }
    // }

}
