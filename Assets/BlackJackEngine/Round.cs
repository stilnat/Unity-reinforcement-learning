using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round
{

    public Game game;
    public List<Player> players;
    public Dealer dealer;
    public Deck deck;

    public Round(Game game)
    {
        this.game = game;
        this.players = game.players;
        this.dealer = game.dealer;
        this.deck = game.deck;
    }



    public void StartRound()
    {
        DistributeCards();

    }

    public void PlayersTurn()
    {
        foreach(Player player in players)
        {
            player.Play(deck);
        }
    }

    public void EndRound()
    {
        dealer.CanHitAndHit(deck);
        dealer.play(deck);
        DistributeGainsAndLoses();

        foreach (Player player in players)
        {
            player.clearHands();
        }
        dealer.clearHand();
    }

    public void DistributeGainsAndLoses()
    {
        foreach (Player player in players)
        {
            foreach (Hand hand in player.hands)
            {
                if (DealerBeaten(hand))
                {
                    if (hand.IsBlackjack())
                    {
                        player.Money += (int)(hand.Bet * 2.5);
                    }
                    else
                    {
                        player.Money += hand.Bet * 2;
                    }
                    
                }
                else if (IsPush(hand))
                {
                    player.Money += hand.Bet;
                }
                hand.Bet = 0;
            }
        }
    }

    private bool IsPush(Hand playerHand)
    {
       return playerHand.HandValue() <= 21 && dealer.hand.HandValue() <= 21 && playerHand.HandValue() == dealer.hand.HandValue() ? true : false;
    }

    private bool DealerBeaten(Hand playerHand)
    {
        if(playerHand.HandValue() <= 21 && dealer.hand.HandValue() <= 21)
        {
            return playerHand.HandValue() > dealer.hand.HandValue() ? true : false;
        }
        else if (playerHand.HandValue() > 21)
        {
            return false;
        }
        else
        {
            return true;
        }
        
    }

    private void DistributeCards()
    {
        foreach(Player player in players)
        {
            foreach(Hand hand in player.hands)
            {
                player.CanHitAndHit(deck, hand);
                player.CanHitAndHit(deck, hand);
            }
            
        }

        dealer.CanHitAndHit(deck);
        
    }

    public void DebugLogCardInGame()
    {
        foreach(Player player in players)
        {
            Debug.Log(player.ToString());
            player.DebugLogHands();
        }

        Debug.Log(dealer.ToString());
        dealer.DebugLogHand();
    }



 
}
