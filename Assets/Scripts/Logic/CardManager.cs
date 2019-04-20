using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{

    public Text NameText;
    public Text DescriptionText;
    public Text HealthText;
    public Text AttackText;
    public Image CardGraphicImage;
    public GameObject CardPreview;
    
    [SerializeField]
    private int healthValue = 0;
    public int HealthValue
    {
        get { return healthValue; }

        set
        {
            healthValue = value;
        }
    }

    [SerializeField]
    private int attackValue = 0;
    public int AttackValue
    {
        get { return attackValue; }

        set
        {
            attackValue = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCardInformantion(CardAsset cardAsset)
    {
        if(cardAsset != null)
        {
            NameText.text = cardAsset.CardName;
            DescriptionText.text = cardAsset.Description;
            HealthText.text = cardAsset.Health.ToString();
            AttackText.text = cardAsset.Attack.ToString();
            CardGraphicImage.sprite = cardAsset.CardImage;

            HealthValue = cardAsset.Health;
            AttackValue = cardAsset.Attack;

            if (CardPreview != null)
            {
                CardPreview.GetComponent<CardManager>().UpdateCardInformantion(cardAsset);
            }
        }
        
    }

    public void UpdateHeathCardInformantion(int health)
    {
        HealthValue = health;
        if (CardPreview != null)
        {
            CardPreview.GetComponent<CardManager>().UpdateHeathCardInformantion(health);
        }

    }

    void OnMouseDown()
    {
        Debug.Log("OnMouseDown");

        GameLogicManager gameLogic = GlobalSettings.Instance.GameLogicManager.GetComponent<GameLogicManager>();

        if(gameLogic.CurrentState == GameState.DISTRIBUTION_CARD)
        {
            gameLogic.AddCardToPlayer(this.gameObject);
            gameLogic.UpdatePlayerCardPostion();
        }
        else if (gameLogic.CurrentState == GameState.BATTLE)
        {
            if(!gameLogic.IsCardBattle(this.gameObject) && !gameLogic.IsFighting())
            {
                gameLogic.MovePlayerCardToBattlePoint(this.gameObject);
                gameLogic.UpdatePlayerCardPostion();
            }
        }
    }

    public int HealthValueAfterBattle(int amount)
    {
        return healthValue - amount;
    }
}
