using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameTypes;

public class HandRankManager
{
    private HandRankManager()
    {

    }

    private static HandRankManager _instance;
    public static HandRankManager GetHandRankManager()
    {
        if (_instance == null)
            _instance = new HandRankManager();

        return _instance;
    }

    public Ranking EvaluateRank(List<Card> cards)
    {
        Ranking ranking = new Ranking();
        ranking.cards = new List<Card>();

        SortHand(ref cards);

        List<Card> tmpCards = new List<Card>();
        //Check for Royal Flush and Straight Flush from the highest straight
        if (CheckForStraight(cards, ref ranking.cards))
        {
            ranking.rank = Ranking.Rank.STRAIGHT; //Hand is automatically a straight, override if found royal flush or straight flush
            Debug.Log("Hand is a straight");

            tmpCards.Clear();
            tmpCards.AddRange(ranking.cards);
            if (CheckForFlush(tmpCards, ref ranking.cards))
            {
                if (ranking.cards[0].cardValue == Card.Value.ACE)
                {
                    ranking.rank = Ranking.Rank.ROYAL_FLUSH;
                    Debug.Log("Hand is a royal flush");
                }
                else
                {
                    ranking.rank = Ranking.Rank.STRAIGHT_FLUSH;
                    Debug.Log("Hand is a royal flush");
                }
            }
            else
            {
                //Get card subsets for edge case testing
                //Check for edge case straight flush from a possible lower straight
                for (int i = 0; i < cards.Count - 5; i++)
                {
                    List<Card> cardSubset = new List<Card>();
                    int subsetIdx = i + 1;
                    cardSubset.AddRange(cards.GetRange(subsetIdx, cards.Count - subsetIdx));
                    if (CheckForStraight(cardSubset, ref ranking.cards))
                    {
                        tmpCards.Clear();
                        tmpCards.AddRange(ranking.cards);
                        if (CheckForFlush(tmpCards, ref ranking.cards))
                        {
                            ranking.rank = Ranking.Rank.STRAIGHT_FLUSH;
                            Debug.Log("Hand is a straight flush");
                            break;
                        }
                    }
                }
            }
        }
        
        //Check for Four of a Kind

        //Check for Flush
        if (ranking.rank == Ranking.Rank.STRAIGHT && CheckForFlush(cards, ref ranking.cards)) //If previous check found a straight then this found a flush (but not share the same cards) override to flush
        {
            ranking.rank = Ranking.Rank.FLUSH;

            Debug.Log("Hand is a flush");
        }

        //Check for Full House

        //Check Straight -- Check already done in royal/straigh flush check

        //Check for Three of a Kind

        //Check for Two Pairs

        //Check for One Pair

        //Check the High Card

        return ranking;
    }

    private void SortHand(ref List<Card> cardsToSort)
    {
        List<Card> tmpCards = new List<Card>();

        tmpCards.Add(cardsToSort[0]);
        for (int i = 1; i < cardsToSort.Count; i++)
        {
            for (int j = 0; j < tmpCards.Count; j++)
            {
                if ((int)cardsToSort[i].cardValue <= (int)tmpCards[j].cardValue)
                {
                    tmpCards.Insert(j, cardsToSort[i]);

                    break;
                }
                else if (j == tmpCards.Count - 1)
                {
                    tmpCards.Add(cardsToSort[i]);

                    break;
                }

            }
        }

        cardsToSort.Clear();
        cardsToSort.AddRange(tmpCards);
        cardsToSort.Reverse();
    }

    private int[] ComputeValuesDifferences(List<Card> cardsToCompute)
    {
        int[] val = new int[cardsToCompute.Count];
        int[] diff = new int[cardsToCompute.Count - 1];

        for (int i = 0; i < cardsToCompute.Count; i++)
        {
            val[i] = (int)cardsToCompute[i].cardValue;
        }

        for (int i = 0; i < diff.Length; i++)
        {
            diff[i] = val[i] - val[i + 1];
        }

        return diff;
    }

    private bool CheckForStraight(List<Card> cardsToCheck, ref List<Card> participatingCards)
    {
        participatingCards.Clear();
        int straightCtr = 0;
        int[] diff = ComputeValuesDifferences(cardsToCheck);

        //Check from the highest card going down to automatically get the highest possible straight
        for (int i = 0; i < diff.Length; i++)
        {
            if (diff[i] > 1)
            {
                straightCtr = 0;
                participatingCards.Clear();
            }
            else if (diff[i] == 1)
            {
                straightCtr++;
                participatingCards.Add(cardsToCheck[i]);

                if (straightCtr >= 4)
                {
                    participatingCards.Add(cardsToCheck[i + 1]);
                    return true;
                }
                    
            }
        }

        return false;
    }
    
    private bool CheckForFlush(List<Card> cardsToCheck, ref List<Card> participatingCards)
    {
        participatingCards.Clear();
        int[] suitsCtr = new int[(int)Card.Suit.MAX_SUIT];

        for (int i = 0; i < cardsToCheck.Count; i++)
        {
            suitsCtr[(int)cardsToCheck[i].cardSuit]++;
        }

        for(int i = 0; i < suitsCtr.Length; i++)
        {
            if(suitsCtr[i] >= 5)
            {
                for(int j = 0; j < cardsToCheck.Count; j++)
                {
                    if(cardsToCheck[j].cardSuit == (Card.Suit)i)
                    {
                        participatingCards.Add(cardsToCheck[j]);

                        if (participatingCards.Count >= 5)
                            break;
                    }
                }
                return true;
            }
        }

        return false;
    }
}