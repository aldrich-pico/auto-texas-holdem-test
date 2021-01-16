using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameTypes;

public class HandRankManager
{
    List<Card> hand;
    List<Card> tmpCards;

    int[] val;
    int[] diff;
    int straightCtr;
    int straightIdx;
    int straightLowIdx;
    int straightHighIdx;

    Ranking ranking;

    private HandRankManager()
    {
        hand = new List<Card>();
        tmpCards = new List<Card>();

        ranking = new Ranking();

        straightCtr = 0;
        straightIdx = 0;
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
        hand.Clear();
        hand.AddRange(cards);

        SortHand();

        return EvaluatePattern();
    }

    private void SortHand()
    {
        //Sort ascending from index 0
        tmpCards.Clear();

        tmpCards.Add(hand[0]);
        for (int i = 1; i < hand.Count; i++)
        {
            for (int j = 0; j < tmpCards.Count; j++)
            {
                if ((int)hand[i].cardValue <= (int)tmpCards[j].cardValue)
                {
                    tmpCards.Insert(j, hand[i]);

                    break;
                }
                else if(j == tmpCards.Count-1)
                {
                    tmpCards.Add(hand[i]);

                    break;
                }

            }
        }

        hand.Clear();
        hand.AddRange(tmpCards);
    }

    private Ranking EvaluatePattern()
    {
        //Compute for differences between the sorted cards
        ComputeValuesDifferences();

        //Check for Royal Flush
        if (CheckForStraight())
        {
            ranking.rank = Ranking.Rank.STRAIGHT;
            ranking.highCard = hand[straightHighIdx];
            ranking.lowCard = hand[straightLowIdx];

            Debug.Log("Hand is a straight"); //WIP CHECK FOR HIGHCARD ACE, CHECK FOR SAME SUIT
        }
        //Check for Straight Flush

        //Check for Four of a Kind

        //Check for Flush

        //Check for Full House

        //Check Straight

        //Check for Three of a Kind

        //Check for Two Pairs

        //Check for One Pair

        //Check the High Card

        return ranking;
    }

    private void ComputeValuesDifferences()
    {
        val = new int[hand.Count];
        diff = new int[hand.Count - 1];

        for (int i = 0; i < hand.Count; i++)
        {
            val[i] = (int)hand[i].cardValue;
        }

        for (int i = 0; i < diff.Length; i++)
        {
            diff[i] = val[i + 1] - val[i];
        }
    }

    private bool CheckForStraight()
    {
        straightCtr = 0;
        straightLowIdx = 0;
        straightHighIdx = 0;

        for (int i = 0; i < diff.Length; i++)
        {
            if (diff[i] > 1)
                straightCtr = 0;
            else if (diff[i] == 1)
            {
                straightCtr++;
                if (straightCtr == 1)
                    straightLowIdx = i;
                else if (straightCtr >= 4)
                {
                    straightHighIdx = i + 1;

                    return true;
                }
                    
            }
        }

        return false;
    }
}