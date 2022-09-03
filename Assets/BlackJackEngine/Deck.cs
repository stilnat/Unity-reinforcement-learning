using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck 
{

    public Stack<Card> cards;
     

    public Deck(int numberOfPacks = 6)
    {

        cards = new Stack<Card>();
         
        for(int k = 0; k < numberOfPacks; k++)
        {
            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Card card = new Card((Card.CardName)i, (Card.CardType)j);
                    cards.Push(card);
                }
            }
        }   
    }

    public Card DrawCard()
    {
        return cards.Pop();
    }

    public void Shuffle()
    {
        Card[] cardsArray = cards.ToArray();
        int n = cardsArray.Length;
        
        
        while (n > 0)
        {
            int k = Random.Range(0, n - 1);
            Card temp = cardsArray[n-1];
            cardsArray[n-1] = cardsArray[k];
            cardsArray[k] = temp;
            n--;
        }

        cards.Clear();

        foreach(Card card in cardsArray)
        {
            cards.Push(card);
        }    
    }


    public void DebugLogAllCardsInDeck()
    {
        foreach(Card card in cards)
        {
            Debug.Log(card.ToString());
        }
    }

    


}
