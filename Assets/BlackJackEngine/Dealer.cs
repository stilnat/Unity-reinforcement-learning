using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealer
{
    public Hand hand;

    public Dealer()
    {
        hand = new Hand();
    }

    private void Hit(Deck deck)
    {
        Card card = deck.DrawCard();
        hand.cards.Add(card);
    }

    public bool CanHitAndHit(Deck deck)
    {
        if (hand.HandValue() < 21)
        {
            Hit(deck);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DebugLogHand()
    {
        Debug.Log("Dealer Hand : " + hand.ToString());
    }

    public void clearHand()
    {
       hand.cards.Clear();   
    }

    public void play(Deck deck)
    {
        while (hand.HandValue() < 17 || (hand.HandValue() == 17 && hand.containsAce()))
        {
            CanHitAndHit(deck);

        }

    }
}
