using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand
{

    public List<Card> cards;

    private int bet;
    public int Bet
    {
        get { return bet;  }
        set
        {
            bet = value;
        }
    }

    public Hand()
    {
        cards = new List<Card>();
        bet = 0;
    }

    public int HandValue()
    {
        int value = 0;
        foreach(Card card in cards)
        {
            value += card.Value();
        }

        foreach (Card card in cards)
        {
            if(value > 21 && card.GetCardName() == Card.CardName.Ace)
            {
                value -= 10;
            }     
        }

        return value;
    }

    public bool containsAce()
    {
        foreach (Card card in cards)
        {
            if(card.GetCardName() == Card.CardName.Ace)
            {
                return true;
            }
            
        }
        return false;
    }

    public bool IsBlackjack()
    {
        if(cards.Count == 2)
        {
            if(cards[0].GetCardName() == Card.CardName.Ace && cards[1].Value() == 10)
            {
                return true;
            }
            if (cards[1].GetCardName() == Card.CardName.Ace && cards[0].Value() == 10)
            {
                return true;
            }
        }
        return false;
    }

    public override string ToString()
    {
        string content = "";
        foreach(Card card in cards)
        {
            content += card.ToString() + " : ";
        }

        return content;
    }
}
