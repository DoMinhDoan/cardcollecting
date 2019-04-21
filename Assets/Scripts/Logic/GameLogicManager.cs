using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    DISTRIBUTION_CARD,
    BATTLE,
    FIGHTING,
    END_GAME
}

public class GameLogicManager : MonoBehaviour
{
    public List<CardAsset> allCardsAsset = new List<CardAsset>();
    public GameObject CreatureCardPrefab;

    public GameObject PlayerManager;
    public GameObject AIManager;    

    public Transform PlayerBattlePoint;
    public Transform AIBattlePoint;

    public Text ScoreText;
    public GameObject GameOver;
    public Text BattleResult;

    public GameObject PlayerInfo;
    public Text GameOverScoreText;

    [SerializeField]
    private List<GameObject> allCardsObject = new List<GameObject>();

    [SerializeField]
    private GameState currentState = GameState.DISTRIBUTION_CARD;
    public GameState CurrentState
    {
        get { return currentState; }

        set
        {
            currentState = value;
        }
    }

    

    private void Awake()
    {
        LocalSaveManager.Instance.LoadGame();
        ScoreText.text = LocalSaveManager.Instance.GetGameSave().score.ToString();
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
                cardsPositionXOffset += 1.2f;
            }
            GameObject card = GameObject.Instantiate(CreatureCardPrefab, new Vector3(cardsPositionX, 0f, 0f), Quaternion.Euler(new Vector3(0f, -179f, 0f))) as GameObject;
            card.GetComponent<CardManager>().UpdateCardInformation(allCardsAsset[i]);
            card.GetComponent<HoverPreview>().ThisPreviewEnabled = false;

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
        card.GetComponent<HoverPreview>().ThisPreviewEnabled = true;
        card.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        PlayerManager.GetComponent<PlayerManager>().AddCard(card);

        allCardsObject.Remove(card);
        //UpdateListCardPostion();

        if(PlayerManager.GetComponent<PlayerManager>().NumberCard() == GlobalSettings.Instance.MaxPlayerCard)
        {
            TransferRestCardToAI();
            StartCoroutine(ProcessAIToBattle());
        }
    }

    IEnumerator ProcessAIToBattle()
    {
        yield return new WaitForSeconds(2.5f);
        MoveAICardToBattlePoint();  //AI first
        UpdateAiCardPostion();
    }

    void TransferRestCardToAI()
    {
        foreach(var card in allCardsObject)
        {
            card.GetComponent<HoverPreview>().ThisPreviewEnabled = false;
            AIManager.GetComponent<PlayerManager>().AddCard(card);
        }
        allCardsObject.Clear();
        //UpdateListCardPostion();
        UpdateAiCardPostion();

        currentState = GameState.BATTLE;
    }

    public void UpdatePlayerCardPostion()
    {
        PlayerManager.GetComponent<PlayerManager>().UpdateCardPosition();
    }

    public void UpdateAiCardPostion()
    {
        AIManager.GetComponent<PlayerManager>().UpdateCardPosition();
    }

    public void MovePlayerCardToBattlePoint(GameObject card)
    {
        PlayerManager.GetComponent<PlayerManager>().AddCardToBattle(card, PlayerBattlePoint);

        if (IsFighting() && currentState != GameState.FIGHTING)
        {
            currentState = GameState.FIGHTING;
            StartCoroutine(UpdateFighting());
        }
    }

    public void MoveAICardToBattlePoint()
    {
        if(ShouldEndGame())
        {
            return;
        }
        AIManager.GetComponent<PlayerManager>().AddAICardToBattle(AIBattlePoint);

        if (IsFighting() && currentState != GameState.FIGHTING)
        {
            currentState = GameState.FIGHTING;
            StartCoroutine(UpdateFighting());
        }
    }

    IEnumerator UpdateFighting()
    {
        AIManager.GetComponent<PlayerManager>().EnableCardBattle();

        yield return new WaitForSeconds(2.5f);

        int playerHealthValue = PlayerManager.GetComponent<PlayerManager>().HealthBattleValue() - AIManager.GetComponent<PlayerManager>().AttackBattleValue();
        int aiHealthValue = AIManager.GetComponent<PlayerManager>().HealthBattleValue() - PlayerManager.GetComponent<PlayerManager>().AttackBattleValue();

        if(playerHealthValue > 0 && playerHealthValue > aiHealthValue)
        {
            AIManager.GetComponent<PlayerManager>().DestroyBattleCard();
            PlayerManager.GetComponent<PlayerManager>().AddScore(1);

            ScoreText.text = (LocalSaveManager.Instance.GetGameSave().score + PlayerManager.GetComponent<PlayerManager>().CurrentScore).ToString();

            //LocalSaveManager.Instance.UpdateSaveGame(PlayerManager.GetComponent<PlayerManager>().CurrentScore);

            PlayerManager.GetComponent<PlayerManager>().UpdateBattleHeathCardInformation(playerHealthValue);

            currentState = GameState.BATTLE;

            StartCoroutine(ProcessAIToBattle());
        }
        else if (aiHealthValue > 0 && aiHealthValue > playerHealthValue)
        {
            PlayerManager.GetComponent<PlayerManager>().DestroyBattleCard();
            AIManager.GetComponent<PlayerManager>().AddScore(1);
            AIManager.GetComponent<PlayerManager>().UpdateBattleHeathCardInformation(aiHealthValue);

            currentState = GameState.BATTLE;
        }
        else if(playerHealthValue <= 0 && aiHealthValue <= 0)
        {
            PlayerManager.GetComponent<PlayerManager>().DestroyBattleCard();
            AIManager.GetComponent<PlayerManager>().DestroyBattleCard();

            currentState = GameState.BATTLE;
            StartCoroutine(ProcessAIToBattle());
        }
        else if (playerHealthValue > 0 && aiHealthValue > 0 && playerHealthValue == aiHealthValue)
        {
            AIManager.GetComponent<PlayerManager>().UpdateBattleHeathCardInformation(aiHealthValue);
            PlayerManager.GetComponent<PlayerManager>().UpdateBattleHeathCardInformation(playerHealthValue);

            currentState = GameState.BATTLE;
            StartCoroutine(UpdateFighting());
        }

        if(ShouldEndGame())
        {
            currentState = GameState.END_GAME;
            DeactiveGameField();
            GameOver.SetActive(true);
            LocalSaveManager.Instance.UpdateSaveGame(LocalSaveManager.Instance.GetGameSave().score + PlayerManager.GetComponent<PlayerManager>().CurrentScore);
            LocalSaveManager.Instance.SaveGame();

            if (PlayerManager.GetComponent<PlayerManager>().CurrentScore > AIManager.GetComponent<PlayerManager>().CurrentScore)
            {
                BattleResult.text = "YOU WIN";
            }
            else if (PlayerManager.GetComponent<PlayerManager>().CurrentScore == AIManager.GetComponent<PlayerManager>().CurrentScore)
            {
                BattleResult.text = "Draw!";
            }
            else
            {
                BattleResult.text = "YOU LOSS";
            }
            
        }
    }

    void DeactiveGameField()
    {
        PlayerInfo.SetActive(false);
        GameOverScoreText.text = (LocalSaveManager.Instance.GetGameSave().score + PlayerManager.GetComponent<PlayerManager>().CurrentScore).ToString();

        PlayerManager.GetComponent<PlayerManager>().DeactiveCardAction();
        AIManager.GetComponent<PlayerManager>().DeactiveCardAction();
    }

    public bool IsCardBattle(GameObject card)
    {
        return PlayerManager.GetComponent<PlayerManager>().IsCardBattle(card) || AIManager.GetComponent<PlayerManager>().IsCardBattle(card);
    }

    public bool IsFighting()
    {
        return PlayerManager.GetComponent<PlayerManager>().HasCardBattle() && AIManager.GetComponent<PlayerManager>().HasCardBattle();
    }

    public bool IsEmptyBattle()
    {
        return !PlayerManager.GetComponent<PlayerManager>().HasCardBattle() && !AIManager.GetComponent<PlayerManager>().HasCardBattle();
    }

    public bool ShouldEndGame()
    {
        return (PlayerManager.GetComponent<PlayerManager>().NumberCard() == 0 || AIManager.GetComponent<PlayerManager>().NumberCard() == 0);
    }
}
