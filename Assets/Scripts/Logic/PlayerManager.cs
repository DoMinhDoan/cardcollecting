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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCard(GameObject card)
    {
        cardsInHand.Add(card);
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
}
