using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public bool isPlayer = true;
    public List<CardAsset> cardsAsset = new List<CardAsset>();
    public GameObject CreatureCardPrefab;

    private List<GameObject> cardsInHand = new List<GameObject>();
    private List<float> cardsPositionX = new List<float> { 0, -3f, 3f, -6f, 6f };

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < cardsAsset.Count; i++)    
        {
            GameObject card;
            if(isPlayer)
            {
                card = GameObject.Instantiate(CreatureCardPrefab, new Vector3(cardsPositionX[i], -3f, 0f), Quaternion.Euler(new Vector3(0f, 0f, 0f))) as GameObject;
            }
            else
            {
                card = GameObject.Instantiate(CreatureCardPrefab, new Vector3(cardsPositionX[i], 5f, 0f), Quaternion.Euler(new Vector3(0f, -179f, 0f))) as GameObject;
            }

            AddCard(card);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCard(GameObject card)
    {
        cardsInHand.Add(card);
    }
}
