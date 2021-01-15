using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealer
{
    public const int NUMBER_OF_PLAYERS = 2;
    public const int CARDS_PER_PLAYER = 2;
    public const int NUMBER_OF_COMMUNITY_CARDS = 5;

    private Deck deck;

    List<Deck.Card>[] playerHands = new List<Deck.Card>[NUMBER_OF_PLAYERS];
    List<Deck.Card> communityCards = new List<Deck.Card>();

    private Dealer()
    {
        deck = new Deck();
    }

    private static Dealer _instance;
    public static Dealer GetDealer()
    {
        if (_instance == null)
            _instance = new Dealer();

        return _instance;
    }

    public void DealCards()
    {
        for(int i = 0; i < NUMBER_OF_PLAYERS; i++)
        {
            playerHands[i] = deck.DrawCards(CARDS_PER_PLAYER);
        }
    }

    public void DealCommunityCards()
    {
        communityCards = deck.DrawCards(NUMBER_OF_COMMUNITY_CARDS);
    }
}
