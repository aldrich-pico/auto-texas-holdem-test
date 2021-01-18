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

        /*DEBUG
         {
            Debug.Log("Player 1:");
            Debug.Log(playerHands[0][0].cardValue.ToString()+"--"+ playerHands[0][0].cardSuit.ToString());
            Debug.Log(playerHands[0][1].cardValue.ToString() + "--" + playerHands[0][1].cardSuit.ToString());

            Debug.Log("Player 2:");
            Debug.Log(playerHands[1][0].cardValue.ToString() + "--" + playerHands[1][0].cardSuit.ToString());
            Debug.Log(playerHands[1][1].cardValue.ToString() + "--" + playerHands[1][1].cardSuit.ToString());
        }
         //*/
        return playerHands;
    }

    public List<Card> DealCommunityCards()
    {
        if (communityCards == null)
            communityCards = new List<Card>();
        else
            communityCards.Clear();

        communityCards.AddRange(deck.DrawCards(NUMBER_OF_COMMUNITY_CARDS));

        /*DEBUG
        {
            Debug.Log("Community:");
            Debug.Log(communityCards[0].cardValue.ToString() + "--" + communityCards[0].cardSuit.ToString());
            Debug.Log(communityCards[1].cardValue.ToString() + "--" + communityCards[1].cardSuit.ToString());
            Debug.Log(communityCards[2].cardValue.ToString() + "--" + communityCards[2].cardSuit.ToString());
            Debug.Log(communityCards[3].cardValue.ToString() + "--" + communityCards[3].cardSuit.ToString());
            Debug.Log(communityCards[4].cardValue.ToString() + "--" + communityCards[4].cardSuit.ToString());
        }
        //*/
        return communityCards;
    }

    public void EvaluateHands()
    {
        /*
        //DEBUG
        {
            playerHands[0].Clear();
            playerHands[1].Clear();
            communityCards.Clear();

            playerHands[0].Add(new Card(Card.Suit.CLUB, Card.Value.THREE));
            playerHands[0].Add(new Card(Card.Suit.SPADE, Card.Value.SIX));

            playerHands[1].Add(new Card(Card.Suit.HEART, Card.Value.SIX));
            playerHands[1].Add(new Card(Card.Suit.SPADE, Card.Value.SIX));

            communityCards.Add(new Card(Card.Suit.SPADE, Card.Value.THREE));
            communityCards.Add(new Card(Card.Suit.DIAMOND, Card.Value.THREE));
            communityCards.Add(new Card(Card.Suit.DIAMOND, Card.Value.SIX));
            communityCards.Add(new Card(Card.Suit.DIAMOND, Card.Value.FIVE));
            communityCards.Add(new Card(Card.Suit.HEART, Card.Value.QUEEN));
        {
            Debug.Log("Player 1:");
            Debug.Log(playerHands[0][0].cardValue.ToString()+"--"+ playerHands[0][0].cardSuit.ToString());
            Debug.Log(playerHands[0][1].cardValue.ToString() + "--" + playerHands[0][1].cardSuit.ToString());

            Debug.Log("Player 2:");
            Debug.Log(playerHands[1][0].cardValue.ToString() + "--" + playerHands[1][0].cardSuit.ToString());
            Debug.Log(playerHands[1][1].cardValue.ToString() + "--" + playerHands[1][1].cardSuit.ToString());
        }

        {
            Debug.Log("Community:");
            Debug.Log(communityCards[0].cardValue.ToString() + "--" + communityCards[0].cardSuit.ToString());
            Debug.Log(communityCards[1].cardValue.ToString() + "--" + communityCards[1].cardSuit.ToString());
            Debug.Log(communityCards[2].cardValue.ToString() + "--" + communityCards[2].cardSuit.ToString());
            Debug.Log(communityCards[3].cardValue.ToString() + "--" + communityCards[3].cardSuit.ToString());
            Debug.Log(communityCards[4].cardValue.ToString() + "--" + communityCards[4].cardSuit.ToString());
        }
        }
        //*/

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
