using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    List<Card> deckCardList = new List<Card>();
    List<Card> dealtCards = new List<Card>();
    List<Card> cards = new List<Card>();

    public Deck()
    {
        CreateDeck();
    }

    private void CreateDeck()
    {
        deckCardList.Clear();

        for (int i = 0; i < (int)Card.Suit.MAX_SUIT; i++)
        {
            for (int j = 0; j < (int)Card.Value.MAX_VALUE; j++)
            {
                deckCardList.Add(new Card((Card.Suit)i, (Card.Value)j));
            }
        }

        Debug.Log("Deck Created! Cards: " + deckCardList.Count);
    }

    public List<Card> DrawCards(int numberOfCards)
    {
        cards.Clear();

        for(int i = 0; i < numberOfCards; i++)
        {
            int cardIdx = Random.Range(0, deckCardList.Count);
            cards.Add(deckCardList[cardIdx]);
            dealtCards.Add(deckCardList[cardIdx]);
            deckCardList.RemoveAt(cardIdx);
        }

        return cards;
    }

    public void ResetDeck()
    {
        dealtCards.Clear();
        cards.Clear();

        CreateDeck();
    }

    public struct Card
    {
        public enum Suit
        {
            HEART,
            SPADE,
            CLUB,
            DIAMOND,
            MAX_SUIT
        }

        public enum Value
        {
            TWO, 
            THREE, 
            FOUR, 
            FIVE, 
            SIX, 
            SEVEN, 
            EIGHT, 
            NINE, 
            TEN, 
            JACK, 
            QUEEN, 
            KING, 
            ACE,
            MAX_VALUE
        }

        public Suit cardSuit;
        public Value cardValue;

        public Card(Suit suit, Value value)
        {
            cardSuit = suit;
            cardValue = value;
        }
    }
}
