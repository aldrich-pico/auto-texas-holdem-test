using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameTypes;

public class Dealer
{
    public const int NUMBER_OF_PLAYERS = 2;
    public const int CARDS_PER_PLAYER = 2;
    public const int NUMBER_OF_COMMUNITY_CARDS = 5;

    private Deck deck;

    public List<Card>[] playerHands = new List<Card>[NUMBER_OF_PLAYERS];
    public List<Card> communityCards = new List<Card>();
    public Ranking[] rankings = new Ranking[NUMBER_OF_PLAYERS];

    public RankManager.Result result;

    private Dealer()
    {
        deck = new Deck();
        result = RankManager.Result.DRAW;
    }

    private static Dealer _instance;
    public static Dealer GetDealer()
    {
        if (_instance == null)
            _instance = new Dealer();

        return _instance;
    }

    public List<Card>[] DealCards()
    {
        deck.ResetDeck();

        for (int i = 0; i < NUMBER_OF_PLAYERS; i++)
        {
            if (playerHands[i] == null)
                playerHands[i] = new List<Card>();
            else
                playerHands[i].Clear();

            playerHands[i].AddRange(deck.DrawCards(CARDS_PER_PLAYER));
        }

        return playerHands;
    }

    public List<Card> DealCommunityCards()
    {
        if (communityCards == null)
            communityCards = new List<Card>();
        else
            communityCards.Clear();

        communityCards.AddRange(deck.DrawCards(NUMBER_OF_COMMUNITY_CARDS));

        return communityCards;
    }

    public void EvaluateHands()
    {
        for (int i = 0; i < NUMBER_OF_PLAYERS; i++)
        {
            List<Card> combinedCards = new List<Card>();
            combinedCards.AddRange(playerHands[i]);
            combinedCards.AddRange(communityCards);

            rankings[i] = RankManager.GetHandRankManager().EvaluateRank(combinedCards);
        }
    }

    public void EvaluateWinner()
    {
        result = RankManager.GetHandRankManager().CompareRanks(rankings[0], rankings[1]);
    }
}
