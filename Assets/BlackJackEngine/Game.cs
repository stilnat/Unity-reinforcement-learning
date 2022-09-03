using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    private int numberOfPacks;
    private int numberOfPlayers;
    public List<Player> players;
    public Dealer dealer;
    public Deck deck;

    // Start is called before the first frame update
    void Start()
    {
        numberOfPacks = 6;
        numberOfPlayers = 1;
        players = new List<Player>();
        dealer = new Dealer();
        for (int i = 0; i < numberOfPlayers; i++)
        {
            players.Add(new Player(500, 1));
        }

        deck = new Deck(numberOfPacks);
        deck.Shuffle();

            while (!IsGameOver())
            {
                Debug.Log("before round player's money = " + players[0].Money);
                Round round = new Round(this);
                round.StartRound();
                round.PlayersTurn();
                round.EndRound();
                Debug.Log("after round player's money = " + players[0].Money);

            }
    }

    public bool IsGameOver()
    {
        return deck.cards.Count < 80 ? true : false ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
