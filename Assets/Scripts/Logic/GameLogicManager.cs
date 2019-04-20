using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicManager : MonoBehaviour
{
    public List<CardAsset> allCardsAsset = new List<CardAsset>();
    public GameObject CreatureCardPrefab;

    public GameObject PlayerManager;
    public GameObject AIManager;

    [SerializeField]
    private List<GameObject> allCardsObject = new List<GameObject>();

    private void Awake()
    {
        allCardsAsset.Shuffle();
    }
    // Start is called before the first frame update
    void Start()
    {
        float cardsPositionXOffset = 0;
        for (int i = 0; i < allCardsAsset.Count; i++)
        {
            float cardsPositionX = cardsPositionXOffset;
            if (i % 2 != 0)
            {
                cardsPositionX = -cardsPositionXOffset;
            }
            else
            {
                cardsPositionXOffset += 1.5f;
            }
            GameObject card = card = GameObject.Instantiate(CreatureCardPrefab, new Vector3(cardsPositionX, 0f, 0f), Quaternion.Euler(new Vector3(0f, 0f, 0f))) as GameObject;
            card.GetComponent<CardManager>().UpdateCardInformantion(allCardsAsset[i]);

            allCardsObject.Add(card);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateListCardPostion()
    {
        float cardsPositionXOffset = 0;
        for (int i = 0; i < allCardsObject.Count; i++)
        {
            float cardsPositionX = cardsPositionXOffset;
            if (i % 2 != 0)
            {
                cardsPositionX = -cardsPositionXOffset;
            }
            else
            {
                cardsPositionXOffset += 1.5f;
            }

            allCardsObject[i].transform.localPosition = new Vector3(cardsPositionX, 0f, 0f);
        }
    }

    public void AddCardToPlayer(GameObject card)
    {
        PlayerManager.GetComponent<PlayerManager>().AddCard(card);

        allCardsObject.Remove(card);
        UpdateListCardPostion();

        if(PlayerManager.GetComponent<PlayerManager>().NumberCard() == GlobalSettings.Instance.MaxPlayerCard)
        {
            TransferRestCardToAI();
        }
    }
    void TransferRestCardToAI()
    {
        foreach(var card in allCardsObject)
        {
            AIManager.GetComponent<PlayerManager>().AddCard(card);
        }
        allCardsObject.Clear();
        UpdateListCardPostion();
        UpdateAiCardPostion();
    }

    public void UpdatePlayerCardPostion()
    {
        PlayerManager.GetComponent<PlayerManager>().UpdateCardPosition();
    }

    public void UpdateAiCardPostion()
    {
        AIManager.GetComponent<PlayerManager>().UpdateCardPosition();
    }
}
