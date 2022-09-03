using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{

    public enum CardName
    {
        Ace,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King
    }

    public enum CardType
    {
        Club,
        Spade,
        Heart,
        Diamond
    }

    private CardName cardName;
    private CardType cardType;

    bool isVisible;

    public CardName GetCardName()
    {
        return cardName;
    }

    public Card(CardName name, CardType type)
    {
        this.cardName = name;
        this.cardType = type;
        isVisible = false;

    }

    public int Value()
    {
        switch (cardName)
        {
            case CardName.Ace:
                return 11;
            case CardName.Two:
                return 2;
            case CardName.Three:
                return 3;
            case CardName.Four:
                return 4;
            case CardName.Five:
                return 5;
            case CardName.Six:
                return 6;
            case CardName.Seven:
                return 7;
            case CardName.Eight:
                return 8;
            case CardName.Nine:
                return 9;
            case CardName.Ten:
                return 10;
            case CardName.Jack:
                return 10;
            case CardName.Queen:
                return 10;
            case CardName.King:
                return 10;

            default: return 0;
        }
    }

    public override string ToString()
    {
        return cardType.ToString() + " " + cardName.ToString();
    }

    
}
