using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public bool isPlayer = true;
    public List<CardAsset> cardsAsset = new List<CardAsset>();
    public GameObject CreatureCardPrefab;

    [SerializeField]
    private List<GameObject> cardsInHand = new List<GameObject>();

    [SerializeField]
    private GameObject cardInBattle = null;

    [SerializeField]
    private int currentScore = 0;
    public int CurrentScore
    {
        get { return currentScore; }

        set
        {
            currentScore = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentScore = GlobalSettings.Instance.DefaultPlayerScore;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCard(GameObject card)
    {
        cardsInHand.Add(card);
    }

    void RemoveCard(GameObject card)
    {
        cardsInHand.Remove(card);
    }

    public int NumberCard()
    {
        return cardsInHand.Count;
    }

    public void UpdateCardPosition()
    {
        float cardsPositionXOffset = 0;
        for (int i = 0; i < cardsInHand.Count; i++)
        {
            float cardsPositionX = cardsPositionXOffset;
            if (i % 2 != 0)
            {
                cardsPositionX = -cardsPositionXOffset;
            }
            else
            {
                cardsPositionXOffset += 2f;
            }

            if (isPlayer)
            {
                cardsInHand[i].transform.localPosition = new Vector3(cardsPositionX, -3f, 0f);
            }
            else
            {
                cardsInHand[i].transform.localPosition = new Vector3(cardsPositionX, 5f, 0f);
                cardsInHand[i].transform.localRotation = Quaternion.Euler(new Vector3(0f, -179f, 0f));
            }
        }
    }

    public void AddCardToBattle(GameObject card, Transform tranform)
    {
        cardInBattle = card;
        cardInBattle.transform.localPosition = tranform.localPosition;

        RemoveCard(card);
    }

    public void AddAICardToBattle(Transform tranform)
    {
        int aiCardRandrom = Random.Range(0, cardsInHand.Count);
        GameObject card = cardsInHand[aiCardRandrom];

        cardInBattle = card;
        cardInBattle.transform.localPosition = tranform.localPosition;

        RemoveCard(card);
    }

    public void EnableCardBattle()
    {
        cardInBattle.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    }

    public bool IsCardBattle(GameObject card)
    {
        return cardInBattle == card;
    }

    public bool HasCardBattle()
    {
        return cardInBattle != null;
    }

    public int AttackBattleValue()
    {
        return cardInBattle.GetComponent<CardManager>().AttackValue;
    }

    public int HealthBattleValue()
    {
        return cardInBattle.GetComponent<CardManager>().HealthValue;
    }

    public void DestroyBattleCard()
    {
        Destroy(cardInBattle);
        cardInBattle = null;
    }

    public void AddScore(int amount)
    {
        CurrentScore += amount;
    }

    public void UpdateBattleHeathCardInformation(int health)
    {
        cardInBattle.GetComponent<CardManager>().UpdateHeathCardInformation(health);
    }
}
