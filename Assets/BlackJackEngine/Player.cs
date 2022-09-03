using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{

    public List<Hand> hands;
    public int baseNbHands;
    private int money;
    private string name;


    public int Money
    {
        get { return money; }
        set { money = value; }
    }
    

    public Player(int money = 200, int nbHands = 1, string name = "John Doe")
    {
        baseNbHands = nbHands;
        hands = new List<Hand>();
        for(int i=0; i<nbHands; i++)
        {
            hands.Add(new Hand());
        }
       
        this.money = money;
        this.name = name;
    }

    public override string ToString()
    {
        return "Player " + this.name + " : " + this.money + "$";
    }

    private void Hit(Deck deck, Hand hand)
    {
        Card card = deck.DrawCard();
        hand.cards.Add(card);
    }

    public bool CanHitAndHit(Deck deck, Hand hand)
    {
        if (hand.HandValue() <= 21)
        {
            Hit(deck, hand);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DebugLogHands()
    {
        int i = 0;
        foreach(Hand hand in hands)
        {
            Debug.Log("Player Hand number " + i + ": " + hand.ToString());
            i++;
        }
    }

    public void clearHands()
    {
        foreach (Hand hand in hands)
        {
            hand.cards.Clear();
        }
        while(hands.Count > baseNbHands)
        {
            hands.RemoveAt(0);
        }
    }

    public void bet(Hand hand, int amount)
    {
        if(money >= amount)
        {
            hand.Bet = amount;
            money -= amount;
        }
        else
        {
            throw new System.Exception("The bet is more than the money left.");
        }   
    }

    public void Play(Deck deck)
    {
        foreach(Hand hand in hands)
        {
            PlayDealerStrategy(deck, hand);
            bet(hand, 10);
        }
    }

    public void PlayDealerStrategy(Deck deck, Hand hand)
    {
        while (hand.HandValue() < 17 || (hand.HandValue() == 17 && hand.containsAce()))
        {
            CanHitAndHit(deck, hand);

        }
    }


}
