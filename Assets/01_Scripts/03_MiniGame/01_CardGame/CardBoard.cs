using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class CardBoard : MonoBehaviour
{
    [SerializeField]
    Card cardPrefab;

    private List<GameObject> cards;

    public int cardCount = 16;
    public int rowCount = 4;

    private int nowCorrectCount = 0;

    [SerializeField] private Transform board;
    [SerializeField] private Transform topLeft;
    [SerializeField] private Transform topRight;
    [SerializeField] private Transform bottomLeft;
    [SerializeField] private Transform bottomRight;


    public void SpawnCards()
    {
        int[] arr = new int[cardCount];
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = i / 2;
        }

        arr = arr.OrderBy(x => Random.Range(0f, 7f)).ToArray();

        float cardBoardWidth = Vector3.Distance(topRight.position, topLeft.position);
        float cardBoardHeight = Vector3.Distance(topRight.position, bottomRight.position);


        float cardRowDis = cardBoardWidth * 0.1f / (rowCount - 1);
        float cardColumnDis = cardBoardHeight * 0.1f / (cardCount / rowCount - 1);
        float cardWidth = (cardBoardWidth / rowCount) - cardRowDis;
        float cardHeight = cardBoardHeight / (cardCount / rowCount) - cardColumnDis;

        cards = new List<GameObject>();

        for (int i = 0; i < cardCount; ++i)
        {
            Vector3 pos = new Vector3(
                cardWidth * (i % rowCount)
                + (cardRowDis * (i % rowCount))
                + (cardWidth * 0.5f) + (cardRowDis/2),
                0.02f,
                cardHeight * (i / rowCount)
                + (cardColumnDis * (i / rowCount))
                + (cardHeight * 0.5f)+ (cardColumnDis/2)
            );

            Card nowCard = Instantiate(cardPrefab);
            nowCard.OnOpenCardAction += () => { OpenCard(nowCard); };

            nowCard.transform.position = bottomLeft.position + pos;
            cards.Add(nowCard.gameObject);

            nowCard.SetIndex(arr[i]);
            nowCard.transform.localScale = new Vector3(cardWidth, 1, cardHeight);
            nowCard.transform.SetParent(board);
            //nowCard.transform.localPosition = pos;
        }
    }


    private bool secondOpen = false;
    private Card preOpenCard;

    public void OpenCard(Card card)
    {
        if (secondOpen)
        {
            if (card.idx == preOpenCard.idx)
            {
                nowCorrectCount += 2;
                //SoundManager.GetInstance().PlayOnce("match");
                if (nowCorrectCount == cardCount)
                {
                    Time.timeScale = 0;
                    CardMinigame.Instance.GameClear();
                }
            }
            else
            {
                StartCoroutine(CloseCard(preOpenCard, card));
            }
            secondOpen = false;
        }
        else
        {
            preOpenCard = card;
            secondOpen = true;
        }

    }

    IEnumerator CloseCard(Card preCard, Card nowCard)
    {
        yield return new WaitForSeconds(0.25f);
        preCard.CloseCard();
        nowCard.CloseCard();
    }


    public void OpenAllCard()
    {
        for (int i = 0; i < cards.Count; ++i)
        {
            cards[i].GetComponent<Card>().back.SetActive(true);
            cards[i].GetComponent<Card>().front.SetActive(false);
        }
    }
}
