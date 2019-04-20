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

            if(CardPreview != null)
            {
                CardPreview.GetComponent<CardManager>().UpdateCardInformantion(cardAsset);
            }
        }
        
    }
}
