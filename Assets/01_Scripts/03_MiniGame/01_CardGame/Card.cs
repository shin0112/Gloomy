using System;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int idx { get; private set; }

    public GameObject front;
    public GameObject back;

    public event Action OnOpenCardAction; 
    void Start()
    {   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetIndex(int _idx)
    {
        idx = _idx;
     
        // Sprite spr = Resources.Load<Sprite>("Sprite/" + "rtan" + idx.ToString());
        // back.GetComponent<Image>().sprite = spr;
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
