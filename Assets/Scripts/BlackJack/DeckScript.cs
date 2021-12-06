using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckScript : MonoBehaviour
{
    public Sprite[] cardSprites;
    int[] cardValues = new int[53];
    int currentIdex = 0;

    void Start()
    {
        GetCardValues();
    }

    void GetCardValues()
    {
        int num = 0;
        for(int i = 0; i<cardSprites.Length; i++)
        {
            num = i;
            num %= 13;
            if( num>10 || num == 00)
            {
                num = 10;
            }
            cardValues[i] = num++;
        }
        currentIdex = 1;
    }

    public void Shuffle()
    {
        for(int i = cardSprites.Length -1; i > 0; --i )
        {
            int j = Mathf.FloorToInt(Random.Range(0.0f, 1.0f) * cardSprites.Length - 1) + 1;
            Sprite face = cardSprites[i];
            cardSprites[i] = cardSprites[j];
            cardSprites[j] = face;

            int value = cardValues[i];
            cardValues[i] = cardValues[j];
            cardValues[j] = value;
        }
    }

    public int DealCard(CardScript cardScript)
    {
        if(currentIdex > 52)
        {
            currentIdex = 1;
        }

        cardScript.SetSprite(cardSprites[currentIdex]);
        cardScript.SetValue(cardValues[currentIdex]);
        currentIdex++;
        return cardScript.GetValueOfCard();
    }

    public Sprite GetCardBack()
    {
        return cardSprites[0];
    }
}
