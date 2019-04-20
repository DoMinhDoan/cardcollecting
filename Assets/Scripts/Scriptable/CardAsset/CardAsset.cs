using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardAsset : ScriptableObject 
{	
    [Header("Card Info")]
    [TextArea(2,3)]
    public string Description;
	public Sprite CardImage;
    public int Health;
    public int Attack;
}
